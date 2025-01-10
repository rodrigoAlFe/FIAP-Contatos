using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Repositories;

namespace FIAP.Contatos.Service.Services
{

    public class ContatoService(ContatoRepository contatoRepository)
    {
        public async Task<List<Contato?>> GetContatosAsync(int? ddd = null)
        {
            return await contatoRepository.GetAllAsync(ddd);
        }

        public async Task AddContatoAsync(Contato? contato)
        {
            await contatoRepository.AddAsync(contato);
        }

        public async Task UpdateContatoAsync(Contato? contato)
        {
            await contatoRepository.UpdateAsync(contato);
        }

        public async Task DeleteContatoAsync(int id)
        {
            await contatoRepository.DeleteAsync(id);
        }
    }

}
