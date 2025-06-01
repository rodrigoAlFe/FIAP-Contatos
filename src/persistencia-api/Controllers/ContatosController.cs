using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using persistencia_api.Data;
using persistencia_api.Entities;

namespace persistencia_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContatosController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    // GET: api/Contatos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contato>>> GetContatos([FromQuery] int? ddd)
    {
        if (ddd.HasValue)
        {
            return await _context.Contatos.Where(c => c.Ddd == ddd.Value).ToListAsync();
        }
        return await _context.Contatos.ToListAsync();
    }

    // GET: api/Contatos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Contato>> GetContato(int id)
    {
        var contato = await _context.Contatos.FindAsync(id);
        if (contato == null) return NoContent();
        return contato;
    }

    // POST: api/Contatos
    [HttpPost]
    public async Task<ActionResult<Contato>> PostContato(Contato contato)
    {
        _context.Contatos.Add(contato);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetContato), new { id = contato.Id }, contato);
    }

    // PUT: api/Contatos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutContato(int id, Contato contato)
    {
        if (id != contato.Id) return BadRequest();
        _context.Entry(contato).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ContatoExists(id)) return NotFound();
            else throw;
        }
        return NoContent();
    }

    // DELETE: api/Contatos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContato(int id)
    {
        var contato = await _context.Contatos.FindAsync(id);
        if (contato == null) return NotFound();
        _context.Contatos.Remove(contato);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> ContatoExists(int id)
    {
        return await _context.Contatos.AnyAsync(e => e.Id == id);
    }
}

