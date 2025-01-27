using Payments.Domain;
using Payments.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Payment.API.MessageBroker
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;

        public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            var factory = new RabbitMQ.Client.ConnectionFactory { HostName = "localhost" }; // Change if RabbitMQ is remote
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange and queue
            _channel.ExchangeDeclare(exchange: "billing_exchange", type: ExchangeType.Fanout);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "billing_exchange", routingKey: "");

            _logger.LogInformation("RabbitMQConsumer initialized and ready to consume messages.");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Message received: {Message}", message);

                try
                {
                    var billEvent = JsonSerializer.Deserialize<dynamic>(message);
                    await ProcessMessageAsync(billEvent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message.");
                }
            };

            _channel.BasicConsume(queue: _channel.QueueDeclare().QueueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(dynamic billEvent)
        {
            if (billEvent?.Event == "BillCreated")
            {
                // Extract data
                var billData = billEvent?.Data;

                // Use DI to resolve your repository or services
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

                // Add the bill to Payment database
                dbContext.Bills.Add(new Bill
                {
                    BillID = (Guid)billData?.BillID,
                    UserID = (Guid)billData?.UserID,
                    Amount = (decimal)billData?.Amount,
                    DueDate = DateTime.Parse((string)billData?.DueDate),
                    IsPaid = (bool)billData?.IsPaid
                });

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Bill added to Payment database.");
            }
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
