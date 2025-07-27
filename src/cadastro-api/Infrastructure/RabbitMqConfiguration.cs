namespace cadastro_api.Infrastructure;

public class RabbitMqConfiguration
{
    public string ConnectionString { get; set; } = "amqp://admin:admin123@localhost:5672/";
    public string ExchangeName { get; set; } = "contatos-exchange";
    public string QueueName { get; set; } = "contatos-queue";
    public string RoutingKey { get; set; } = "contato.created";
}