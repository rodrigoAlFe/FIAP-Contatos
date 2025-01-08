using FIAP.Contatos.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FIAP.Contatos.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Construtor que recebe as opções do DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Definição do DbSet que representa a tabela "Contatos" no banco de dados
        public DbSet<Contato> Contatos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(b => b.MigrationsAssembly("FIAP.Contatos"));
        }

    }

}
