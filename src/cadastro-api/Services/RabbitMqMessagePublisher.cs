using cadastro_api.Infrastructure;
using cadastro_api.Messages;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace cadastro_api.Services;

public class RabbitMqMessagePublisher : IMessagePublisher, IDisposable
{
    private readonly RabbitMqConfiguration _config;
    private readonly ILogger<RabbitMqMessagePublisher> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMqMessagePublisher(IOptions<RabbitMqConfiguration> config, ILogger<RabbitMqMessagePublisher> logger)
    {
        _config = config.Value;
        _logger = logger;
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_config.ConnectionString)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare exchange
            _channel.ExchangeDeclare(
                exchange: _config.ExchangeName,
                type: ExchangeType.Direct,
                durable: true);

            // Declare queue
            _channel.QueueDeclare(
                queue: _config.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind queue to exchange
            _channel.QueueBind(
                queue: _config.QueueName,
                exchange: _config.ExchangeName,
                routingKey: _config.RoutingKey);

            _logger.LogInformation("RabbitMQ initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize RabbitMQ");
            throw;
        }
    }

    public async Task PublishContatoCreatedAsync(ContatoCreatedMessage message)
    {
        try
        {
            if (_channel?.IsOpen != true)
            {
                _logger.LogWarning("RabbitMQ channel is not open, reinitializing...");
                InitializeRabbitMq();
            }

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel!.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = Guid.NewGuid().ToString();
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(
                exchange: _config.ExchangeName,
                routingKey: _config.RoutingKey,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Published contato creation message with ID: {MessageId}", properties.MessageId);
            
            await Task.CompletedTask; // For async interface consistency
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish contato creation message");
            throw;
        }
    }

    public void Dispose()
    {
        try
        {
            _channel?.Close();
            _connection?.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing RabbitMQ resources");
        }
    }
}