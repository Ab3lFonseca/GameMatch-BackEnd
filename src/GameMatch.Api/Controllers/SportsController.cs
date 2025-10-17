using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GameMatch.Api.Controllers;

[ApiController]
[Route("api/sports")]
[Authorize] // ou remova se quiser deixar público
public class SportsController : ControllerBase
{
    private readonly AppDb _db;
    public SportsController(AppDb db) { _db = db; }

    // GET /api/sports
    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Sports.Include(s => s.Positions).ToListAsync());

    // GET /api/sports/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sport = await _db.Sports.Include(s => s.Positions)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (sport == null) return NotFound(new { mensagem = "Esporte não encontrado" });
        return Ok(sport);
    }

    // POST /api/sports
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Sport sport)
    {
        if (await _db.Sports.AnyAsync(s => s.Name == sport.Name))
            return BadRequest(new { mensagem = "Esporte já existe" });

        _db.Sports.Add(sport);
        await _db.SaveChangesAsync();
        return Created($"/api/sports/{sport.Id}", sport);
    }

    // PUT /api/sports/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Sport dto)
    {
        var sport = await _db.Sports.FindAsync(id);
        if (sport == null) return NotFound();

        sport.Name = dto.Name;
        sport.Description = dto.Description;
        sport.IconUrl = dto.IconUrl;
        await _db.SaveChangesAsync();

        return Ok(sport);
    }

    // DELETE /api/sports/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var sport = await _db.Sports.FindAsync(id);
        if (sport == null) return NotFound();
        _db.Sports.Remove(sport);
        await _db.SaveChangesAsync();
        return Ok(new { mensagem = "Esporte removido" });
    }
}
