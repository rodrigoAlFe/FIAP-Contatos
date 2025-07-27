using Microsoft.EntityFrameworkCore;
using persistencia_api.Data;
using persistencia_api.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace persistencia_api.Services;

public class RabbitMqContatoConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMqContatoConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration.GetValue<string>("RabbitMq:Host") ?? "localhost",
            UserName = _configuration.GetValue<string>("RabbitMq:User") ?? "admin",
            Password = _configuration.GetValue<string>("RabbitMq:Pass") ?? "admin",
            Port = _configuration.GetValue<int?>("RabbitMq:Port") ?? 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "contatos", durable: false, exclusive: false, autoDelete: false, arguments: null);

        return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel!);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            ContatoFilaDTO? dto = null;
            try
            {
                dto = JsonSerializer.Deserialize<ContatoFilaDTO>(message);
            }
            catch
            {
                Console.WriteLine("Erro ao desserializar mensagem: " + message);
                _channel!.BasicNack(ea.DeliveryTag, false, false);
                return;
            }

            if (dto != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (dto.Acao == "create" && dto.Contato != null)
                {
                    db.Contatos.Add(dto.Contato);
                    await db.SaveChangesAsync();
                }
                else if (dto.Acao == "update" && dto.Contato != null)
                {
                    db.Entry(dto.Contato).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                else if (dto.Acao == "delete" && dto.Id.HasValue)
                {
                    var contato = await db.Contatos.FindAsync(dto.Id.Value);
                    if (contato != null)
                    {
                        db.Contatos.Remove(contato);
                        await db.SaveChangesAsync();
                    }
                }
            }

            _channel!.BasicAck(ea.DeliveryTag, false);
        };

        _channel!.BasicConsume(queue: "contatos", autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}