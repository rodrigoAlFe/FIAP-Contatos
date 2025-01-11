using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Repositories;

namespace FIAP.Contatos.Service.Services
{

    public class ContatoService(ContatoRepository contatoRepository)
    {
        public async Task<List<Contato?>> GetAllContatosAsync()
            => await contatoRepository.GetAllAsync();

        public async Task<List<Contato?>> GetAllByDDDAsync(int? ddd)
            => await contatoRepository.GetAllByDDDAsync(ddd);

        public async Task<Contato?> GetByIdAsync(int id)
            => await contatoRepository.GetByIdAsync(id);

        public async Task AddContatoAsync(Contato? contato)
            => await contatoRepository.AddAsync(contato);

        public async Task UpdateContatoAsync(Contato? contato)
            => await contatoRepository.UpdateAsync(contato);

        public async Task DeleteContatoAsync(int id)
            => await contatoRepository.DeleteAsync(id);
    }

}
