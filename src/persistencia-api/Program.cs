using Microsoft.EntityFrameworkCore;
using persistencia_api.Data;
using persistencia_api.Infrastructure;
using persistencia_api.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure RabbitMQ
builder.Services.Configure<RabbitMqConfiguration>(
    builder.Configuration.GetSection("RabbitMq"));

// Register message consumer as hosted service
builder.Services.AddHostedService<ContatoMessageConsumer>();

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

app.UseHttpMetrics();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapMetrics();

app.Run();

public partial class Program { }