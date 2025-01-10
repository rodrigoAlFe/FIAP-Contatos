using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Contatos.Infrastructure.Repositories
{

    public class ContatoRepository(ApplicationDbContext context)
    {
        public async Task<List<Contato>?> GetAllAsync(int? ddd = null)
        {
            return await context.Contatos
                                 .Where(c => !ddd.HasValue || c.Ddd == ddd.Value)
                                 .ToListAsync();
        }

        public async Task<Contato?> GetByIdAsync(int id)
        {
            return await context.Contatos.FindAsync(id);
        }

        public async Task AddAsync(Contato? contato)
        {
            await context.Contatos.AddAsync(contato);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contato? contato)
        {
            context.Contatos.Update(contato);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contato = await GetByIdAsync(id);
            if (contato != null)
            {
                context.Contatos.Remove(contato);
                await context.SaveChangesAsync();
            }
        }
    }

}
