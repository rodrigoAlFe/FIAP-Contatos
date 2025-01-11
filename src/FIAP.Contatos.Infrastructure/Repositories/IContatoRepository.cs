using FIAP.Contatos.Domain.Entities;

namespace FIAP.Contatos.Infrastructure.Repositories;

public interface IContatoRepository
{
    Task<List<Contato?>> GetAllAsync();
    Task<List<Contato?>> GetAllByDDDAsync(int? ddd);
    Task<Contato?> GetByIdAsync(int id);
    Task AddAsync(Contato? contato);
    Task UpdateAsync(Contato? contato);
    Task DeleteAsync(int id);
}