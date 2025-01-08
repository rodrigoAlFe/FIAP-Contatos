using FIAP.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Infrastructure.Context;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
    }
}