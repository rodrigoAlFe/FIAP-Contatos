using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Cache;
using FIAP.Contatos.Services;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.Contatos.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : ControllerBase
    {
        private readonly ContatoService _contatoService;
        private readonly ContatoCache _contatoCache;

        public ContatosController(ContatoService contatoService, ContatoCache contatoCache)
        {
            _contatoService = contatoService;
            _contatoCache = contatoCache;
        }

        [HttpGet("/BuscaContatos")]
        public async Task<ActionResult<List<Contato>>> GetContatos([FromQuery] int? ddd)
        {
            List<Contato>? contatos = await _contatoCache.Get();
            if (contatos.Count > 0)
            {
                if (ddd != null)
                    return Ok(contatos.Where(x => x.Ddd == ddd));

                return Ok(contatos);
            }

            contatos = await _contatoService.GetContatosAsync(ddd);
            _contatoCache.Set(contatos);

            return Ok(contatos);
        }

        [HttpPost("/InsereContato")]
        public async Task<IActionResult> CreateContato([FromBody] Contato contato)
        {
            bool success = await _contatoService.AddContatoAsync(contato);

            if (!success)
            {
                return Conflict(new { Message = "Um contato com o Id informado já existe" });
            }

            _contatoCache.Set(await _contatoService.GetContatosAsync());
            return CreatedAtAction(nameof(GetContatos), new { id = contato.Id }, contato);
        }

        [HttpPut("/AtualizaContato{id}")]
        public async Task<IActionResult> UpdateContato(int id, [FromBody] Contato contato)
        {
            if (id != contato.Id)
                return BadRequest();

            await _contatoService.UpdateContatoAsync(contato);
            _contatoCache.Set(await _contatoService.GetContatosAsync());
            return NoContent();
        }

        [HttpDelete("/DeletaContato{id}")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            await _contatoService.DeleteContatoAsync(id);
            _contatoCache.Set(await _contatoService.GetContatosAsync());
            return NoContent();
        }
    }
}
