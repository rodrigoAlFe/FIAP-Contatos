using FIAP.Api.Endpoints;
using FIAP.CrossCutting.MiddlewareConfig;
using FIAP.CrossCutting.ServicesConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Services
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .InstallServices(builder.Configuration, typeof(IServiceInstaller).Assembly);

var app = builder.Build();

// Add middlewares
app.ConfigureMiddleware(builder.Configuration, typeof(IApplicationMiddleware).Assembly);

app.UseHttpsRedirection();

app.MapContactEndpoints();

app.Run();