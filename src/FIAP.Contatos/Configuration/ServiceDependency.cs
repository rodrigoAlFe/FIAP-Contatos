using FIAP.Contatos.Service.Services;

namespace FIAP.Contatos.Configuration
{
    public static class ServiceDependency
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<ContatoService, ContatoService>();
            return services;
        }
    }
}
