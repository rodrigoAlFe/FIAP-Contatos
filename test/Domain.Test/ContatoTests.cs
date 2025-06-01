using System.ComponentModel.DataAnnotations;
using persistencia_api.Entities;

namespace Domain.Test;

public class ContatoTests
{
    [Fact]
    public void Contato_DeveSerValido_QuandoTodosOsCamposForemPreenchidosCorretamente()
    {
        // Arrange
        var contato = new Contato
        {
            Id = 1,
            Nome = "João Silva",
            Telefone = "1234-5678",
            Email = "joao@exemplo.com",
            Ddd = 11
        };

        // Act
        var validationResults = ValidateModel(contato);

        // Assert
        Assert.Empty(validationResults); // Sem erros de validação
    }

    [Theory]
    [InlineData(null, "1234-5678", "joao@exemplo.com", 11, "O Campo Nome é obrigatório.")]
    [InlineData("João Silva", "abc1234", "joao@exemplo.com", 11, "Telefone no formato inválido.")]
    [InlineData("João Silva", "1234-5678", "email_invalido", 11, "Endereço de e-mail inválido.")]
    [InlineData("João Silva", "1234-5678", "joao@exemplo.com", 123, "DDD no formato inválido.")]
    public void Contato_DeveSerInvalido_ComDadosIncorretos(string? nome, string telefone, string email, int ddd, string mensagemEsperada)
    {
        // Arrange
        var contato = new Contato
        {
            Id = 1,
            Nome = nome,
            Telefone = telefone,
            Email = email,
            Ddd = ddd
        };

        // Act
        var validationResults = ValidateModel(contato);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == mensagemEsperada);
    }

    // Método auxiliar para realizar validações de modelo
    private static IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
        Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
        return validationResults;
    }
}