using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace FIAP.CrossCutting.MiddlewareConfig;

public interface IApplicationMiddleware
{
    void SetupMiddleware(WebApplication app, IConfiguration configuration);
}