using FIAP.Contatos.Infrastructure.Repositories;

namespace FIAP.Contatos.Configuration
{
    public static class RepositoryDependency
    {
        public static IServiceCollection AddRepositoriesDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<ContatoRepository, ContatoRepository>();
            return services;
        }
    }
}
