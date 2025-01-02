using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Repositories;
using FIAP.Contatos.Services;
using Moq;
using Xunit;

namespace FIAP.Contatos.tests.Application.Tests
{

    public class ContatoServiceTests
    {
        private readonly Mock<ContatoRepository> _repoMock;
        private readonly ContatoService _service;

        public ContatoServiceTests()
        {
            _repoMock = new Mock<ContatoRepository>([]);
            _service = new ContatoService(_repoMock.Object);
        }
        
        [Fact]
        public async Task GetContatos_ShouldReturnList()
        {
            var contatos = new List<Contato>
            {
                new Contato { Id = 1, Nome = "Contato 1", Telefone = "992490060", Email = "teste@gmail.com", Ddd = 69 },
                new Contato { Id = 2, Nome = "Contato 2", Telefone = "992490060", Email = "teste@gmail.com", Ddd = 69 }
            };

            _repoMock.Setup(r => r.GetAllAsync(1)).ReturnsAsync(contatos);

            var result = await _service.GetContatosAsync();

            Assert.Equal(2, result.Count);
        }
    }

}
