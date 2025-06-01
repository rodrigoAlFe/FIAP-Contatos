using persistencia_api.Entities;
using Moq;

namespace Infrastructure.Test;

public class ContatosRepositoryTests
{
    private readonly Mock<IContatoRepository> _repositoryMock = new();

    // Mock do repositório

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosOsContatos()
    {
        // Arrange
        var contatosMock = new List<Contato>
        {
            new Contato { Id = 1, Nome = "João Silva", Telefone = "1234-5678", Email = "joao@teste.com", Ddd = 11 },
            new Contato { Id = 2, Nome = "Maria Oliveira", Telefone = "9876-5432", Email = "maria@teste.com", Ddd = 21 }
        };
        _repositoryMock.Setup(repo => repo.GetAllAsync())!.ReturnsAsync(contatosMock);

        // Act
        var contatos = await _repositoryMock.Object.GetAllAsync();

        // Assert
        Assert.NotNull(contatos);
        Assert.Equal(2, contatos.Count);
    }

    [Theory]
    [InlineData(11, 1)]
    [InlineData(21, 1)]
    [InlineData(99, 0)]
    public async Task GetAllAsync_ComFiltroDDD_DeveRetornarContatosEsperados(int ddd, int expectedCount)
    {
        // Arrange
        var contatosMock = new List<Contato>
        {
            new Contato { Id = 1, Nome = "João Silva", Telefone = "1234-5678", Email = "joao@teste.com", Ddd = 11 },
            new Contato { Id = 2, Nome = "Maria Oliveira", Telefone = "9876-5432", Email = "maria@teste.com", Ddd = 21 }
        };

        _repositoryMock
            .Setup(repo => repo.GetAllByDDDAsync(ddd))!
            .ReturnsAsync(contatosMock.Where(c => c.Ddd == ddd).ToList());

        // Act
        var contatos = await _repositoryMock.Object.GetAllByDDDAsync(ddd);

        // Assert
        Assert.NotNull(contatos);
        Assert.Equal(expectedCount, contatos.Count);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarContatoQuandoExistir()
    {
        // Arrange
        var contatoMock = new Contato { Id = 1, Nome = "João Silva" };
        _repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(contatoMock);

        // Act
        var contato = await _repositoryMock.Object.GetByIdAsync(1);

        // Assert
        Assert.NotNull(contato);
        Assert.Equal("João Silva", contato.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNuloQuandoNaoExistir()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Contato)null!);

        // Act
        var contato = await _repositoryMock.Object.GetByIdAsync(99);

        // Assert
        Assert.Null(contato);
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarContato()
    {
        // Arrange
        var novoContato = new Contato
        {
            Id = 3,
            Nome = "Ana Souza",
            Telefone = "1234-1234",
            Email = "ana@teste.com",
            Ddd = 22
        };

        _repositoryMock.Setup(repo => repo.AddAsync(novoContato)).Verifiable();

        // Act
        await _repositoryMock.Object.AddAsync(novoContato);

        // Assert
        _repositoryMock.Verify(repo => repo.AddAsync(novoContato), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarContato()
    {
        // Arrange
        var contatoParaAtualizar = new Contato { Id = 1, Nome = "João Silva" };
        _repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(contatoParaAtualizar);
        _repositoryMock.Setup(repo => repo.UpdateAsync(contatoParaAtualizar)).Verifiable();

        contatoParaAtualizar.Nome = "João Atualizado";

        // Act
        await _repositoryMock.Object.UpdateAsync(contatoParaAtualizar);

        // Assert
        _repositoryMock.Verify(repo => repo.UpdateAsync(contatoParaAtualizar), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverContato()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteAsync(1)).Verifiable();

        // Act
        await _repositoryMock.Object.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveNaoAlterarQuandoContatoNaoExistir()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteAsync(99)).Verifiable();

        // Act
        await _repositoryMock.Object.DeleteAsync(99);

        // Assert
        _repositoryMock.Verify(repo => repo.DeleteAsync(99), Times.Once);
    }
}
