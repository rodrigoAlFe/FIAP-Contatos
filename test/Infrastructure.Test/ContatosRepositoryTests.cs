using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using persistencia_api.Entities;
using Xunit;

namespace IntegrationTests;

public class ContatosRepositoryTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private const string BaseUrl = "/api/persistencia/contatos";

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosOsContatos()
    {
        // Arrange
        var newContato = new Contato
        {
            Nome = "Leonardo",
            Telefone = "12345678",
            Email = "email@teste.com",
            Ddd = 11
        };

        // Act
        var response = await _client.PostAsJsonAsync(BaseUrl, newContato);
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"{BaseUrl}?ddd=11");
        getResponse.EnsureSuccessStatusCode();

        var contatos = await getResponse.Content.ReadFromJsonAsync<List<Contato>>();

        // Assert
        Assert.NotNull(contatos);
        Assert.Contains(contatos!, c => c.Nome == "Leonardo" && c.Ddd == 11);
    }

    [Theory]
    [InlineData(11)]
    [InlineData(21)]
    public async Task GetAllByDDDAsync_DeveRetornarContatosPorDDD(int ddd)
    {
        // Arrange
        var contato = new Contato
        {
            Nome = $"ContatoDDD{ddd}",
            Telefone = "9999-9999",
            Email = $"ddd{ddd}@teste.com",
            Ddd = ddd
        };
        var postResponse = await _client.PostAsJsonAsync(BaseUrl, contato);
        postResponse.EnsureSuccessStatusCode();

        // Act
        var response = await _client.GetAsync($"{BaseUrl}?ddd={ddd}");
        response.EnsureSuccessStatusCode();

        var contatos = await response.Content.ReadFromJsonAsync<List<Contato>>();

        // Assert
        Assert.NotNull(contatos);
        Assert.All(contatos!, c => Assert.Equal(ddd, c.Ddd));
        Assert.Contains(contatos, c => c.Nome == $"ContatoDDD{ddd}");
    }

    [Fact]
    public async Task AddAsync_DeveCriarContato()
    {
        var novoContato = new Contato
        {
            Nome = "Ana Souza",
            Telefone = "12341234",
            Email = "ana@teste.com",
            Ddd = 22
        };

        var response = await _client.PostAsJsonAsync(BaseUrl, novoContato);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var contatoCriado = await response.Content.ReadFromJsonAsync<Contato>();
        Assert.NotNull(contatoCriado);
        Assert.Equal("Ana Souza", contatoCriado!.Nome);

        // Cleanup
        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{contatoCriado.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarContato()
    {
        // Arrange
        var novoContato = new Contato { Nome = "Teste", Telefone = "12345678", Email = "email@teste.com", Ddd = 11 };
        var postResponse = await _client.PostAsJsonAsync(BaseUrl, novoContato);
        var criado = await postResponse.Content.ReadFromJsonAsync<Contato>();

        // Act
        var response = await _client.GetAsync($"{BaseUrl}/{criado!.Id}");
        response.EnsureSuccessStatusCode();

        var contato = await response.Content.ReadFromJsonAsync<Contato>();

        Assert.NotNull(contato);
        Assert.Equal(criado.Id, contato!.Id);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverContato()
    {
        // Arrange
        var novoContato = new Contato { Nome = "Para Deletar", Telefone = "12345678", Email = "email@teste.com", Ddd = 31 };
        var postResponse = await _client.PostAsJsonAsync(BaseUrl, novoContato);
        var criado = await postResponse.Content.ReadFromJsonAsync<Contato>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{criado!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Assert que ele não existe mais
        var getResponse = await _client.GetAsync($"{BaseUrl}/{criado.Id}");
        Assert.Equal(HttpStatusCode.NoContent, getResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarContato()
    {
        // Arrange
        var novoContato = new Contato { Nome = "Atualizar", Telefone = "12345678", Email = "email@teste.com", Ddd = 44 };
        var postResponse = await _client.PostAsJsonAsync(BaseUrl, novoContato);
        var criado = await postResponse.Content.ReadFromJsonAsync<Contato>();

        // Act: atualiza o nome
        criado!.Nome = "Atualizado";
        var putResponse = await _client.PutAsJsonAsync($"{BaseUrl}/{criado.Id}", criado);

        // A resposta do PUT deve ser NoContent (204)
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

        // Busca novamente
        var getResponse = await _client.GetAsync($"{BaseUrl}/{criado.Id}");
        getResponse.EnsureSuccessStatusCode();
        var atualizado = await getResponse.Content.ReadFromJsonAsync<Contato>();

        // Assert
        Assert.NotNull(atualizado);
        Assert.Equal("Atualizado", atualizado!.Nome);
    }
}
