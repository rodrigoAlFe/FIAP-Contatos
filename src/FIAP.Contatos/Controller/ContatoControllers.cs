using FIAP.Contatos.Domain.Entities;
using FIAP.Contatos.Infrastructure.Cache;
using FIAP.Contatos.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.Contatos.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController(ContatoService contatoService, ContatoCache contatoCache) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Contato>>> GetContatos([FromQuery] int? ddd)
        {
            var contatos = await contatoCache.Get()!;

            if (contatos is { Count: > 0 })
                return Ok(ddd != null ? contatos.Where(x => x.Ddd == ddd) : contatos);

            contatoCache.Set(await contatoService.GetAllByDDDAsync(ddd));

            return Ok(contatos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContato([FromBody] Contato? contato)
        {
            await contatoService.AddContatoAsync(contato);
            contatoCache.Set(await contatoService.GetAllContatosAsync());

            if (contato != null) return CreatedAtAction(nameof(GetContatos), new { id = contato.Id }, contato);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateContato(int id, [FromBody] Contato? contato)
        {
            if (id != contato.Id)
                return BadRequest();

            try
            {
                await contatoService.UpdateContatoAsync(contato);
                contatoCache.Set(await contatoService.GetAllContatosAsync());
                return NoContent();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            try
            {
                await contatoService.DeleteContatoAsync(id);
                contatoCache.Set(await contatoService.GetAllContatosAsync());
                return NoContent();

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }            
        }
    }
}
