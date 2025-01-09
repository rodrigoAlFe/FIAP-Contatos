using FIAP.Domain.Requests.ModelRequests;

namespace Domain.Test.Request;

public class ContactRequestTest
{
    [Theory]
    [InlineData("João das Coves", "teste@teste.com", "98765-4321", "21", true)]
    [InlineData("João das Coves", "teste@teste.com", "3456-7890", "21", true)]
    [InlineData("João das Coves  ", "teste@teste.com", "3456-7890", "21", false, "Name.Value")]
    [InlineData("", "teste@teste.com", "3456-7890", "21", false, "Name.Value")]
    [InlineData("  ", "teste@teste.com", "3456-7890", "21", false, "Name.Value")]
    [InlineData(" Coves", "teste@teste.com", "3456-7890", "21", false, "Name.Value")]
    [InlineData("João das Coves", "", "3456-7890", "21", false, "Email.Value")]
    [InlineData("João das Coves", "  ", "3456-7890", "21", false, "Email.Value")]
    [InlineData("João das Coves", "teste.teste.com", "3456-7890", "21", false, "Email.Value")]
    [InlineData("João das Coves", "teste@testecom", "3456-7890", "21", false, "Email.Value")]
    [InlineData("João das Coves", "teste@teste.com", "", "21", false, "Phone.Value")]
    [InlineData("João das Coves", "teste@teste.com", "  ", "21", false, "Phone.Value")]
    [InlineData("João das Coves", "teste@teste.com", "993456-7890", "21", false, "Phone.Value")]
    [InlineData("João das Coves", "teste@teste.com", "934567890", "21", false, "Phone.Value")]
    [InlineData("João das Coves", "teste@teste.com", "93456-890", "21", false, "Phone.Value")]
    [InlineData("João das Coves", "teste@teste.com", "3456-7890", "", false, "Ddd.Value")]
    [InlineData("João das Coves", "teste@teste.com", "3456-7890", "  ", false, "Ddd.Value")]
    [InlineData("João das Coves", "teste@teste.com", "3456-7890", "021", false, "Ddd.Value")]
    public void ContactRequest_MustCreateValidObject_or_thrownError
        (string name, string email, string phone, string ddd, bool expected, string invalidParam = "")
    {
        // Arrange
        var request = new ContactRequest(1, name, email, phone, ddd);

        // Act
        var (isValid, errors) = request.Validate();

        // Assert
        if (expected)
        {
            Assert.True(isValid);
            Assert.Empty(errors);
            Assert.Equal(name, request.Name);
            Assert.Equal(email, request.Email);
            Assert.Equal(phone, request.Phone);
            Assert.Equal(ddd, request.Ddd);
        }
        else
        {
            Assert.False(isValid);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains(invalidParam));
        }
    }
}