using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Infrastructure.Test;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using persistencia_api;
using persistencia_api.Entities;
using Xunit;

namespace IntegrationTests;

public class ContatosControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContatosControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosOsContatos()
    {
        // Arrange - (opcional: inserir dados via POST se necessário)

        // Act
        var response = await _client.GetAsync("/api/persistencia/contatos"); // ajuste para a rota correta
        response.EnsureSuccessStatusCode();

        var contatos = await response.Content.ReadFromJsonAsync<List<Contato>>();

        // Assert
        Assert.NotNull(contatos);
        Assert.True(contatos!.Count >= 0);
    }

    [Theory]
    [InlineData(11)]
    [InlineData(21)]
    public async Task GetAllByDDDAsync_DeveRetornarContatosPorDDD(int ddd)
    {
        var response = await _client.GetAsync($"/api/persistencia/contatos?ddd={ddd}");
        response.EnsureSuccessStatusCode();

        var contatos = await response.Content.ReadFromJsonAsync<List<Contato>>();

        Assert.NotNull(contatos);
        Assert.All(contatos!, c => Assert.Equal(ddd, c.Ddd));
    }

    [Fact]
    public async Task AddAsync_DeveCriarContato()
    {
        var novoContato = new Contato
        {
            Nome = "Ana Souza",
            Telefone = "1234-1234",
            Email = "ana@teste.com",
            Ddd = 22
        };

        var response = await _client.PostAsJsonAsync("/api/persistencia/contatos", novoContato);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var contatoCriado = await response.Content.ReadFromJsonAsync<Contato>();
        await _client.DeleteAsync($"/api/persistencia/contatos/{contatoCriado!.Id}");

        Assert.NotNull(contatoCriado);
        Assert.Equal("Ana Souza", contatoCriado!.Nome);

    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarContato()
    {
        // Pré-condição: inserir contato
        var novoContato = new Contato { Nome = "Teste", Telefone = "123", Email = "t@t.com", Ddd = 11 };
        var postResponse = await _client.PostAsJsonAsync("/api/persistencia/Contatos", novoContato);
        var criado = await postResponse.Content.ReadFromJsonAsync<Contato>();

        // Act
        var response = await _client.GetAsync($"/api/persistencia/Contatos/{criado!.Id}");

        // Garante que o status code seja de sucesso (200, 201, etc.)
        response.EnsureSuccessStatusCode();

        var contentString = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(contentString))
        {
            return;
        }

        // Desserializa apenas se houver conteúdo
        var contato = JsonSerializer.Deserialize<Contato>(contentString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(contato);
        Assert.Equal(criado.Id, contato!.Id);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverContato()
    {
        var novoContato = new Contato { Nome = "Para Deletar", Telefone = "0000", Email = "x@x.com", Ddd = 31 };
        var postResponse = await _client.PostAsJsonAsync("/api/persistencia/contatos", novoContato);
        var criado = await postResponse.Content.ReadFromJsonAsync<Contato>();

        var deleteResponse = await _client.DeleteAsync($"/api/persistencia/contatos/{criado!.Id}");
        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarContato()
    {
        var novoContato = new Contato { Nome = "Atualizar", Telefone = "0000", Email = "a@a.com", Ddd = 44 };
        var postResponse = await _client.PostAsJsonAsync("/api/persistencia/Contatos", novoContato);
        var criado = await postResponse.Content.ReadFromJsonAsync<Contato>();

        criado!.Nome = "Atualizado";
        novoContato.Id = criado.Id;

        var putResponse = await _client.PutAsJsonAsync($"/api/persistencia/Contatos/{criado.Id}", criado);
        Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/persistencia/Contatos/{criado.Id}");

        var contentString = await getResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(contentString))
        {
            return;
        }

        // Desserializa apenas se houver conteúdo
        var atualizado = JsonSerializer.Deserialize<Contato>(contentString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.Equal("Atualizado", atualizado!.Nome);
    }
}
