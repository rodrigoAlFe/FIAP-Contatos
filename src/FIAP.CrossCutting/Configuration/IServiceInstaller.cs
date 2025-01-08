using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.CrossCutting.Configuration;

public interface IServiceInstaller
{
    void SetupServices(IServiceCollection services, IConfiguration configuration);
}