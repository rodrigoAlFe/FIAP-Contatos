using cadastro_api.DTOs;
using cadastro_api.Messages;
using cadastro_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace cadastro_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContatosController(
    IHttpClientFactory httpClientFactory, 
    ILogger<ContatosController> logger,
    IMessagePublisher messagePublisher) : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<ContatosController> _logger = logger;
    private readonly IMessagePublisher _messagePublisher = messagePublisher;

    // GET api/Contatos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var httpClient = _httpClientFactory.CreateClient("PersistenciaApiClient");
        try
        {
            var response = await httpClient.GetAsync("/api/persistencia/contatos");
            if (response.IsSuccessStatusCode)
            {
                var contatos = await response.Content.ReadFromJsonAsync<IEnumerable<ContatoDTO>>();
                return Ok(contatos);
            }
            _logger.LogError("Erro ao chamar Persistencia API (GET All): {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede ao chamar Persistencia API (GET All)");
            return StatusCode(503, "Serviço de persistência indisponível.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar contatos");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    // POST api/Contatos
    [HttpPost]
    public async Task<IActionResult> CreateContato([FromBody] ContatoDTO contatoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Create message and publish to RabbitMQ
            var message = new ContatoCreatedMessage
            {
                Nome = contatoDto.Nome,
                Telefone = contatoDto.Telefone,
                Email = contatoDto.Email,
                Ddd = contatoDto.Ddd
            };

            await _messagePublisher.PublishContatoCreatedAsync(message);
            
            _logger.LogInformation("Contato creation message published for: {Nome}", contatoDto.Nome);
            
            // Return 202 Accepted since this is async processing
            return Accepted(new { Message = "Contato creation request received and is being processed." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing contato creation message");
            return StatusCode(500, "Erro interno ao processar solicitação de criação de contato.");
        }
    }

    // GET api/Contatos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetContatoById(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("PersistenciaApiClient");
        try
        {
            var response = await httpClient.GetAsync($"/api/persistencia/contatos/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contato = await response.Content.ReadFromJsonAsync<ContatoDTO>();
                return Ok(contato);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            _logger.LogError("Erro ao chamar Persistencia API (GET by ID): {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede ao chamar Persistencia API (GET by ID)");
            return StatusCode(503, "Serviço de persistência indisponível.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar contato por ID");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    // PUT api/Contatos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContato(int id, [FromBody] ContatoDTO contatoDto)
    {
        if (id != contatoDto.Id)
        {
            return BadRequest("ID do contato não corresponde ao parâmetro da URL.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var httpClient = _httpClientFactory.CreateClient("PersistenciaApiClient");
        var content = new StringContent(JsonSerializer.Serialize(contatoDto), Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PutAsync($"/api/persistencia/contatos/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return NoContent();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            _logger.LogError("Erro ao chamar Persistencia API (PUT): {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede ao chamar Persistencia API (PUT)");
            return StatusCode(503, "Serviço de persistência indisponível.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao atualizar contato");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    // DELETE api/Contatos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContato(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("PersistenciaApiClient");
        try
        {
            var response = await httpClient.DeleteAsync($"/api/persistencia/contatos/{id}");
            if (response.IsSuccessStatusCode)
            {
                return NoContent();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            _logger.LogError("Erro ao chamar Persistencia API (DELETE): {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede ao chamar Persistencia API (DELETE)");
            return StatusCode(503, "Serviço de persistência indisponível.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao deletar contato");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }
}
