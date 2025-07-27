using cadastro_api.Messages;

namespace cadastro_api.Services;

public interface IMessagePublisher
{
    Task PublishContatoCreatedAsync(ContatoCreatedMessage message);
}