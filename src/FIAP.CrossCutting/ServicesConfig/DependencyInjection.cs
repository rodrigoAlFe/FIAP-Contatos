using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.CrossCutting.ServicesConfig;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices
    (this IServiceCollection services
        , IConfiguration configuration
        , params Assembly[] assemblies)
    {
        var serviceInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(assemblies =>
                assemblies.ImplementedInterfaces.Contains(typeof(IServiceInstaller)))
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        foreach (var service in serviceInstallers)
            service.SetupServices(services, configuration);

        return services;
    }
}