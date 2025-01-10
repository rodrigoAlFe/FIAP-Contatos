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
            List<Contato>? contatos = await contatoCache.Get();
            if (contatos.Count > 0)
            {
                if (ddd != null)
                    return Ok(contatos.Where(x => x.Ddd == ddd));
                
                return Ok(contatos);
            }

            contatos = await contatoService.GetContatosAsync(ddd);
            contatoCache.Set(contatos);

            return Ok(contatos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContato([FromBody] Contato contato)
        {
            await contatoService.AddContatoAsync(contato);
            contatoCache.Set(await contatoService.GetContatosAsync());
            return CreatedAtAction(nameof(GetContatos), new { id = contato.Id }, contato);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContato(int id, [FromBody] Contato contato)
        {
            if (id != contato.Id)
                return BadRequest();

            await contatoService.UpdateContatoAsync(contato);
            contatoCache.Set(await contatoService.GetContatosAsync());
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            await contatoService.DeleteContatoAsync(id);
            contatoCache.Set(await contatoService.GetContatosAsync());
            return NoContent();
        }
    }
}
