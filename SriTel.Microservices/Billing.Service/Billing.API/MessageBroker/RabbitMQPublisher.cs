using Billing.API.MessageBroker.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

public class RabbitMQPublisher : IDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQPublisher(IOptions<RabbitMQSettings> options, ConnectionFactory connectionFactory)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options), "RabbitMQ settings cannot be null.");
        
        // Configure the provided ConnectionFactory
        connectionFactory.HostName = "localhost";
        connectionFactory.UserName = "guest";
        connectionFactory.Password = "guest";

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare the exchange
        _channel.ExchangeDeclare(exchange: "billing_exchange", type: ExchangeType.Fanout);
    }

    public void Publish(string exchange, string message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);

            // Publish the message
            _channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error publishing message: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
