using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace cadastro_api.Services;

public class RabbitMqService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration.GetValue<string>("RabbitMq:Host") ?? "localhost",
            UserName = configuration.GetValue<string>("RabbitMq:User") ?? "admin",
            Password = configuration.GetValue<string>("RabbitMq:Pass") ?? "admin",
            Port = configuration.GetValue<int?>("RabbitMq:Port") ?? 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "contatos", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Publish<T>(T mensagem)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensagem));
        _channel.BasicPublish(exchange: "", routingKey: "contatos", basicProperties: null, body: body);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}