using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FIAP.CrossCutting.MiddlewareConfig;

public class SwaggerMiddlewareInstaller : IApplicationMiddleware
{
    public void SetupMiddleware(WebApplication app, IConfiguration configuration)
    {
        // Add Swagger on development env only.
        if (!app.Environment.IsDevelopment()) return;
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIAP API V1");
            c.RoutePrefix = string.Empty; // Access Swagger on root.
        });
    }
}