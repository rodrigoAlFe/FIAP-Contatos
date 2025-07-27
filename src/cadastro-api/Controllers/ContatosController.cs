using cadastro_api.DTOs;
using cadastro_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace cadastro_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContatosController(IHttpClientFactory httpClientFactory, ILogger<ContatosController> logger, RabbitMqService rabbitMqService) : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<ContatosController> _logger = logger;
    private readonly RabbitMqService _rabbitMqService = rabbitMqService;

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
    public IActionResult CreateContato([FromBody] ContatoDTO contatoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var msg = new ContatoFilaDTO
        {
            Acao = "create",
            Contato = contatoDto
        };

        try
        {
            _rabbitMqService.Publish(msg);
            return Accepted(new { message = "Contato enviado para processamento." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar contato na fila RabbitMQ (POST)");
            return StatusCode(500, "Erro ao enviar para fila.");
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
    public IActionResult UpdateContato(int id, [FromBody] ContatoDTO contatoDto)
    {
        if (id != contatoDto.Id)
        {
            return BadRequest("ID do contato não corresponde ao parâmetro da URL.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var msg = new ContatoFilaDTO
        {
            Acao = "update",
            Contato = contatoDto
        };

        try
        {
            _rabbitMqService.Publish(msg);
            return Accepted(new { message = "Atualização de contato enviada para processamento." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar atualização na fila RabbitMQ (PUT)");
            return StatusCode(500, "Erro ao enviar para fila.");
        }
    }

    // DELETE api/Contatos/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteContato(int id)
    {
        var msg = new ContatoFilaDTO
        {
            Acao = "delete",
            Id = id
        };

        try
        {
            _rabbitMqService.Publish(msg);
            return Accepted(new { message = "Solicitação de exclusão enviada para processamento." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar exclusão na fila RabbitMQ (DELETE)");
            return StatusCode(500, "Erro ao enviar para fila.");
        }
    }
}
