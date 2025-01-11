using FIAP.Contatos.Configuration;
using FIAP.Contatos.Infrastructure.Cache;
using FIAP.Contatos.Infrastructure.Data;
using FIAP.Contatos.Service.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString, b => b.MigrationsAssembly("FIAP.Contatos.Infrastructure")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ContatoService>();
builder.Services.AddScoped<ContatoCache>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServiceDependencies(builder.Configuration);
builder.Services.AddRepositoriesDependencies(builder.Configuration);

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
