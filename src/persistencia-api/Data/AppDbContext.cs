using Microsoft.EntityFrameworkCore;
using persistencia_api.Entities;

namespace persistencia_api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{        
    public DbSet<Contato> Contatos { get; set; }
}
