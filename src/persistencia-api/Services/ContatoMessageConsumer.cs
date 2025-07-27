using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using persistencia_api.Data;
using persistencia_api.Entities;
using persistencia_api.Infrastructure;
using persistencia_api.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace persistencia_api.Services;

public class ContatoMessageConsumer : BackgroundService
{
    private readonly RabbitMqConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ContatoMessageConsumer> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public ContatoMessageConsumer(
        IOptions<RabbitMqConfiguration> config, 
        IServiceProvider serviceProvider,
        ILogger<ContatoMessageConsumer> logger)
    {
        _config = config.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        InitializeRabbitMq();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var contatoMessage = JsonSerializer.Deserialize<ContatoCreatedMessage>(message);

                if (contatoMessage != null)
                {
                    await ProcessContatoCreatedMessage(contatoMessage);
                    _channel!.BasicAck(ea.DeliveryTag, false);
                    _logger.LogInformation("Processed contato creation message for: {Nome}", contatoMessage.Nome);
                }
                else
                {
                    _logger.LogWarning("Failed to deserialize message: {Message}", message);
                    _channel!.BasicNack(ea.DeliveryTag, false, false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                _channel!.BasicNack(ea.DeliveryTag, false, true); // Requeue on error
            }
        };

        _channel!.BasicConsume(queue: _config.QueueName, autoAck: false, consumer: consumer);
        _logger.LogInformation("ContatoMessageConsumer started listening for messages");

        // Keep the service running
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
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

            _logger.LogInformation("RabbitMQ consumer initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize RabbitMQ consumer");
            throw;
        }
    }

    private async Task ProcessContatoCreatedMessage(ContatoCreatedMessage message)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var contato = new Contato
        {
            Nome = message.Nome,
            Telefone = message.Telefone,
            Email = message.Email,
            Ddd = message.Ddd
        };

        context.Contatos.Add(contato);
        await context.SaveChangesAsync();

        _logger.LogInformation("Created contato with ID: {Id} for: {Nome}", contato.Id, contato.Nome);
    }

    public override void Dispose()
    {
        try
        {
            _channel?.Close();
            _connection?.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing RabbitMQ consumer resources");
        }
        finally
        {
            base.Dispose();
        }
    }
}