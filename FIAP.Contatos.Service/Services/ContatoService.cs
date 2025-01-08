using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Repositories;

namespace FIAP.Contatos.Services
{

    public class ContatoService
    {
        private readonly ContatoRepository _contatoRepository;

        public ContatoService(ContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public async Task<List<Contato>> GetContatosAsync(int? ddd = null)
        {
            return await _contatoRepository.GetAllAsync(ddd);
        }

        public async Task<bool> AddContatoAsync(Contato contato)
        {
            Contato contatoRecebido = await _contatoRepository.GetByIdAsync(contato.Id);
            if (contatoRecebido != null)
            {
                return false;
            }

            await _contatoRepository.AddAsync(contato);
            return true;
        }

        public async Task UpdateContatoAsync(Contato contato)
        {
            await _contatoRepository.UpdateAsync(contato);
        }

        public async Task DeleteContatoAsync(int id)
        {
            await _contatoRepository.DeleteAsync(id);
        }
    }

}
