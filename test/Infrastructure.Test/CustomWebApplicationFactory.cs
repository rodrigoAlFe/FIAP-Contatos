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
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // Remove AppDbContext e DbContextOptions
            var descriptors = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(AppDbContext)
                ).ToList();
            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            // Remove RabbitMQ consumer
            var rabbitDescriptor = services.SingleOrDefault(
                d => d.ImplementationType == typeof(RabbitMqContatoConsumer));
            if (rabbitDescriptor != null)
                services.Remove(rabbitDescriptor);

            var toRemove = services
    .Where(d =>
        (d.ServiceType.FullName?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) ?? false) ||
        (d.ImplementationType?.FullName?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) ?? false) ||
        (d.ServiceType.FullName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) ?? false) ||
        (d.ImplementationType?.FullName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) ?? false) ||
        (d.ServiceType.FullName?.Contains("DbContext", StringComparison.OrdinalIgnoreCase) ?? false) ||
        (d.ImplementationType?.FullName?.Contains("DbContext", StringComparison.OrdinalIgnoreCase) ?? false)
    ).ToList();

            foreach (var d in toRemove) services.Remove(d);


            // Adiciona o banco InMemory
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Garante que o banco está criado
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }

}
