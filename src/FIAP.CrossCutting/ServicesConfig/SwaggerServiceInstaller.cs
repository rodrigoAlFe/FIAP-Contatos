using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FIAP.CrossCutting.ServicesConfig;

public class SwaggerServiceInstaller : IServiceInstaller
{
    public void SetupServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer(); 
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FIAP API",
                Version = "v1",
                Description = "API para gerenciar os recursos do FIAP."
            });
        });
    }
}