using FIAP.Contatos.Infrastructure.Cache;
using FIAP.Contatos.Infrastructure.Data;
using FIAP.Contatos.Infrastructure.Repositories;
using FIAP.Contatos.Service.Services;
using Microsoft.EntityFrameworkCore;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString, b => b.MigrationsAssembly("FIAP.Contatos.Infrastructure")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ContatoRepository, ContatoRepository>();
builder.Services.AddScoped<ContatoService>();
builder.Services.AddScoped<ContatoCache>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware para ordenação de rotas
app.UseRouting();

// Rota para métricas Prometheus (adicionada antes de HTTPS Redirection)
app.UseHttpMetrics();

// CI

// Configura os endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // Define os endpoints das rotas dos controladores
    endpoints.MapMetrics();      // Define o endpoint Prometheus na rota "/metrics"
});

app.Run();
