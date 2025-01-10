using FIAP.Application.Handlers;
using FIAP.Domain.Requests;
using FIAP.Domain.Requests.ModelRequests;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.Api.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactController(ContactServiceHandler handler) : ControllerBase
{
    /// <summary>
    /// Cria um novo contato.
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> CreateContact([FromBody] ContactRequest request)
    {
        var response = await handler.CreateAsync(request);
        return response.IsSuccess ? Ok(response.Data) : BadRequest(response);
    }

    /// <summary>
    /// Atualiza um contato existente.
    /// </summary>
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateContact([FromRoute] uint id, [FromBody] ContactRequest request)
    {
        request.Id = id;
        var response = await handler.UpdateAsync(request);
        return response.IsSuccess ? Ok(response.Data) : BadRequest(response);
    }

    /// <summary>
    /// Deleta um contato existente pelo ID.
    /// </summary>
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteContact([FromRoute] uint id)
    {
        var request = new Request { Id = id };
        var response = await handler.DeleteAsync(request);
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Obtém os detalhes de um contato pelo ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetContactById([FromRoute] uint id)
    {
        var request = new Request { Id = id };
        var response = await handler.GetByIdAsync(request);
        return response.IsSuccess ? Ok(response.Data) : NotFound(response);
    }

    /// <summary>
    /// Obtém uma lista paginada de contatos.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllContacts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var request = new PagedRequest
        {
            PageNumber = page,
            PageSize = pageSize
        };

        var response = await handler.GetAllAsync(request);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}