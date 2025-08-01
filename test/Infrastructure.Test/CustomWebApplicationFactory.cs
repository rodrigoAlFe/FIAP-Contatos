using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using persistencia_api.Data;
using persistencia_api.Services;
using System.Linq;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing"); // força uso do appsettings.Testing.json
        builder.ConfigureServices((context, services) =>
        {
            // Remove possíveis registros antigos do contexto
            var descriptors = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(AppDbContext)
                ).ToList();
            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            // Remove RabbitMQ consumer (opcional, se não quer rodar Rabbit nos testes)
            var rabbitDescriptor = services.SingleOrDefault(
                d => d.ImplementationType == typeof(RabbitMqContatoConsumer));
            if (rabbitDescriptor != null)
                services.Remove(rabbitDescriptor);

            // Remove qualquer provider antigo
            var toRemove = services
                .Where(d =>
                    (d.ServiceType.FullName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (d.ImplementationType?.FullName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (d.ServiceType.FullName?.Contains("DbContext", StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (d.ImplementationType?.FullName?.Contains("DbContext", StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();

            foreach (var d in toRemove) services.Remove(d);

            // Adiciona o banco SQL Server (pega a connection string dos appsettings)
            var connectionString = context.Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Garante que o banco está criado (CUIDADO: Isso executa Migrations no banco real!)
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
