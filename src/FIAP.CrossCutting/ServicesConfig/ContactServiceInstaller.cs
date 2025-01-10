using FIAP.Application.Handlers;
using FIAP.Domain.Handlers;
using FIAP.Infrastructure.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.CrossCutting.ServicesConfig;

public class ContactServiceInstaller : IServiceInstaller
{
    public void SetupServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registra ContactDataHandler como Scoped
        services.AddScoped<IContactHandler, ContactDataHandler>();
        
        // Registra ContactServiceHandler como Scoped
        services.AddScoped<ContactServiceHandler>();
    }
}