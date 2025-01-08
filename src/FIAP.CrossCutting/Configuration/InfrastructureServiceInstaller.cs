using Microsoft.EntityFrameworkCore;
using FIAP.Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.CrossCutting.Configuration;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void SetupServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Não consegue conectar no banco de dados!");

        services.AddDbContext<MainDbContext>(options => { options.UseSqlServer(connectionString); });
    }
}