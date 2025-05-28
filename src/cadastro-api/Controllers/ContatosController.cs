using cadastro_api.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace cadastro_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContatosController(IHttpClientFactory httpClientFactory, ILogger<ContatosController> logger) : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<ContatosController> _logger = logger;

    // POST api/Contatos
    [HttpPost]
    public async Task<IActionResult> CreateContato([FromBody] ContatoDTO contatoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var httpClient = _httpClientFactory.CreateClient("PersistenciaApiClient");
        var content = new StringContent(JsonSerializer.Serialize(contatoDto), Encoding.UTF8, "application/json");

        try
        {
            // Chamar POST da persistencia-api
            var response = await httpClient.PostAsync("/api/contatos", content);

            if (response.IsSuccessStatusCode)
            {
                var createdContato = await response.Content.ReadFromJsonAsync<ContatoDTO>();
                // Retorna 201 Created com a localização e o objeto criado
                return CreatedAtAction(nameof(GetContatoById), new { id = createdContato?.Id ?? 0 }, createdContato);
            }
            else
            {
                _logger.LogError("Erro ao chamar Persistencia API (POST): {StatusCode}", response.StatusCode);
                // Retornar um erro genérico ou o erro da API de persistência
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede ao chamar Persistencia API (POST)");
            return StatusCode(503, "Serviço de persistência indisponível."); // Service Unavailable
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar contato");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    // GET api/Contatos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetContatoById(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("PersistenciaApiClient");
        try
        {
            var response = await httpClient.GetAsync($"/api/contatos/{id}");
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

}
