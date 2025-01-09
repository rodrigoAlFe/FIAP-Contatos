using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace FIAP.CrossCutting.MiddlewareConfig;

public static class MiddlewareConfiguration
{
    public static WebApplication ConfigureMiddleware
    (this WebApplication app
        , IConfiguration configuration
        , params Assembly[] assemblies)
    {
        // Carregar todas as implementações de middlewares a partir dos assemblies fornecidos
        var middlewareInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(assemblies =>
                assemblies.ImplementedInterfaces.Contains(typeof(IApplicationMiddleware)))
            .Select(Activator.CreateInstance)
            .Cast<IApplicationMiddleware>();
        
        // Configurar cada middleware encontrado
        foreach (var middleware in middlewareInstallers)
            middleware.SetupMiddleware(app, configuration);
        
        return app;
    }
}