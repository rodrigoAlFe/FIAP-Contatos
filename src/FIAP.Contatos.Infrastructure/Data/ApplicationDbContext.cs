using FIAP.Contatos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Contatos.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        // Construtor que recebe as opções do DbContext

        // Definição do DbSet que representa a tabela "Contatos" no banco de dados
        public DbSet<Contato> Contatos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(b => b.MigrationsAssembly("FIAP.Contatos.Infrastructure"));
        }

    }

}
