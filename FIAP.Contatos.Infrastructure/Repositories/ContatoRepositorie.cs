using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Contatos.Infrastructure.Repositories
{

    public class ContatoRepository
    {
        private readonly ApplicationDbContext _context;

        public ContatoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contato>> GetAllAsync(int? ddd = null)
        {
            return await _context.Contatos
                                 .Where(c => !ddd.HasValue || c.Ddd == ddd.Value)
                                 .ToListAsync();
        }

        public async Task<Contato> GetByIdAsync(int id)
        {
            return await _context.Contatos.FindAsync(id);
        }

        public async Task AddAsync(Contato contato)
        {
            await _context.Contatos.AddAsync(contato);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contato contato)
        {
            _context.Contatos.Update(contato);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contato = await GetByIdAsync(id);
            if (contato != null)
            {
                _context.Contatos.Remove(contato);
                await _context.SaveChangesAsync();
            }
        }
    }

}
