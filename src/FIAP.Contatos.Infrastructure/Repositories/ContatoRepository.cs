using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Contatos.Infrastructure.Repositories
{

    public class ContatoRepository(ApplicationDbContext context) : IContatoRepository
    {
        public async Task<List<Contato?>> GetAllAsync()
            => await context.Contatos.ToListAsync();

        public async Task<List<Contato?>> GetAllByDDDAsync(int? ddd)
        {
            return await context.Contatos.Where(contatos => ddd.HasValue || (contatos != null && contatos.Ddd == ddd)).ToListAsync();
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
            if (await GetByIdAsync(contato.Id) == null)
                throw new Exception("Id Não existe!");

            var trackedEntity = context.ChangeTracker.Entries<Contato>().FirstOrDefault(e => e.Entity.Id == contato.Id);

            if (trackedEntity != null)
            {
                trackedEntity.State = EntityState.Detached;
            }

            context.Contatos.Update(contato);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contato = await GetByIdAsync(id);

            if (contato == null)
                throw new Exception("Id não existe!");

            context.Contatos.Remove(contato);
            await context.SaveChangesAsync();
        }
    }

}
