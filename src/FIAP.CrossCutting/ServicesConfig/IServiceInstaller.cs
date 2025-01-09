using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.CrossCutting.ServicesConfig;

public interface IServiceInstaller
{
    void SetupServices(IServiceCollection services, IConfiguration configuration);
}