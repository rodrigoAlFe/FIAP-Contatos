using FIAP.Application.Handlers;
using FIAP.Domain.Requests;
using FIAP.Domain.Requests.ModelRequests;

namespace FIAP.Api.Endpoints;

public static class ContactEndpoints
{
    /// <summary>
    /// Extensão para registrar rotas de Contatos.
    /// </summary>
    /// <param name="app">O objeto <see cref="WebApplication"/>.</param>
    public static void MapContactEndpoints(this WebApplication app)
    {
        app.MapPost("/contacts", async (ContactRequest request, ContactServiceHandler handler) =>
        {
            // Chama o método de criação do ContactServiceHandler
            var response = await handler.CreateAsync(request);
            return response.IsSuccess ? Results.Ok(response.Data) : Results.BadRequest(response);
        })
        .WithName("CreateContact");

        app.MapPut("/contacts/{id:int}", async (uint id, ContactRequest request, ContactServiceHandler handler) =>
        {
            request.Id = id;
            var response = await handler.UpdateAsync(request);
            return response.IsSuccess ? Results.Ok(response.Data) : Results.BadRequest(response);
        })
        .WithName("UpdateContact");

        app.MapDelete("/contacts/{id:int}", async (uint id, ContactServiceHandler handler) =>
        {
            var request = new Request { Id = id };
            var response = await handler.DeleteAsync(request);
            return response.IsSuccess ? Results.NoContent() : Results.BadRequest(response);
        })
        .WithName("DeleteContact");

        app.MapGet("/contacts/{id:int}", async (uint id, ContactServiceHandler handler) =>
        {
            var request = new Request { Id = id };
            var response = await handler.GetByIdAsync(request);
            return response.IsSuccess ? Results.Ok(response.Data) : Results.NotFound(response);
        })
        .WithName("GetContactById");

        app.MapGet("/contacts", async (ContactServiceHandler handler, int page = 1, int pageSize = 10) =>
        {
            var request = new PagedRequest
            {
                PageNumber = page,
                PageSize = pageSize
            };

            var response = await handler.GetAllAsync(request);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        })
        .WithName("GetAllContacts");
    }
}