using Polly;
using Polly.Extensions.Http;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient("PersistenciaApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PersistenciaApiUrl"] ?? "http://localhost:5148");
})
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

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

app.MapControllers();

app.MapMetrics();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
     return HttpPolicyExtensions
         .HandleTransientHttpError() // HttpRequestException, 5XX e 408
         .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) // Tentar novamente em caso de 404
         .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), onRetry: (outcome, timespan, retryAttempt, context) =>
         {
             // Log da tentativa
             Console.WriteLine($"Retrying due to {outcome.Result?.StatusCode}. Wait: {timespan.TotalSeconds}s, Attempt: {retryAttempt}");
         });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
     return HttpPolicyExtensions
         .HandleTransientHttpError()
         .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30), onBreak: (result, timeSpan, context) =>
         {
             Console.WriteLine("Circuit breaker opened!");
         }, onReset: (context) =>
         {
             Console.WriteLine("Circuit breaker reset!");
         });
}