using Payments.Infrastructure;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Payments.Domain;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public class RabbitMQConsumer : BackgroundService
{
    private readonly ILogger<RabbitMQConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "billing_exchange", type: ExchangeType.Fanout);
        _channel.QueueDeclare(queue: "payment_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: "payment_queue", exchange: "billing_exchange", routingKey: "");

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
                //var billEvent = JsonSerializer.Deserialize<dynamic>(message);
                await ProcessMessageAsync(message);

                // Acknowledge message
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message.");

                // Requeue the message
                _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        _channel.BasicConsume(queue: "payment_queue", autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }

    private async Task ProcessMessageAsync(string message)
    {
        try
        {
            // Deserialize the message as JsonElement instead of dynamic
            var billEvent = JsonSerializer.Deserialize<JsonElement>(message);

            // Check if the 'Event' property exists
            if (billEvent.TryGetProperty("Event", out var eventType))
            {
                var eventValue = eventType.GetString();
                if (eventValue == "BillCreated")
                {
                    // Check if 'Data' property exists
                    if (billEvent.TryGetProperty("Data", out var billData))
                    {
                        // Proceed with processing the bill data
                        using var scope = _serviceProvider.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

                        Bill bill1 = new Bill();

                        bill1.BillID = Guid.Parse(billData.GetProperty("BillID").GetString());
                            bill1.UserID = Guid.Parse(billData.GetProperty("UserID").GetString());
                            bill1.Amount = billData.GetProperty("Amount").GetDecimal();
                            bill1.DueDate = billData.GetProperty("DueDate").GetDateTime();
                            bill1.BillingDate = billData.GetProperty("BillingDate").GetDateTime();
                            bill1.IsPaid = billData.GetProperty("IsPaid").GetBoolean();
                        dbContext.Bills.Add(bill1);

                        try
{
                            await dbContext.SaveChangesAsync();
                        }
                        catch (DbUpdateException dbEx)
                        {
                            _logger.LogError(dbEx, "Error saving changes to the database.");
                            throw;  // Re-throw or handle accordingly
                        }
                        _logger.LogInformation("Bill added to Payment database.");
                        }
                    else
                    {
                        _logger.LogWarning("The 'Data' property is missing in the message.");
                    }
                }
                else
                {
                    _logger.LogWarning("Event type is not 'BillCreated'.");
                }
            }
            else
            {
                _logger.LogWarning("The 'Event' property is missing in the message.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message.");
            throw;
        }
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
