using System.Text.Json;
using FIAP.Domain.Responses.ModelsResponses;

namespace Domain.Test.Response;

public class ContactResponseTest
{
    [Theory]
    [InlineData("João das Coves", "teste@teste.com", "98765-4321", "21")]
    [InlineData("João das Coves", "teste@teste.com", "3456-7890", "21")]
    public void ContactResponse_MustSerializeJson(string name, string email, string phone, string ddd)
    {
        // Arrange
        var response = new ContactResponse
            (1, name, email, phone, ddd, DateTime.Now, DateTime.Now);
        
        // Act
        var json = JsonSerializer.Serialize(response);
        var deserialized = JsonSerializer.Deserialize<ContactResponse>(json);
        
        // Assert
        Assert.NotNull(json);
        Assert.NotNull(deserialized);
        Assert.Equal(response, deserialized);
    }
}