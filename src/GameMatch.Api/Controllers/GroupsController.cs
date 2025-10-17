using GameMatch.Core.Models;
using GameMatch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameMatch.Api.Controllers;

[ApiController]
[Route("api/groups")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly GroupService _groups;
    public GroupsController(GroupService groups) { _groups = groups; }

    public record CreateGroupDto(string Name, string? Description, int MaxMembers);
    public record DefinePositionDto(int PositionId, int Required, int OpenSpots);

    [HttpPost]
    public async Task<IActionResult> Create(CreateGroupDto dto)
    {
        // OwnerId virá do token; para simplificar aqui só recebe via header x-user-id (ou adapte)
        var ownerIdHeader = HttpContext.Request.Headers["x-user-id"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(ownerIdHeader)) return BadRequest(new { mensagem = "x-user-id header obrigatório" });
        var ownerId = int.Parse(ownerIdHeader);
        var g = await _groups.Create(dto.Name, dto.Description, dto.MaxMembers, ownerId);
        return Ok(g);
    }

    [HttpGet]
    public async Task<IActionResult> List() => Ok(await _groups.List());

    [HttpPost("{id:int}/positions")]
    public async Task<IActionResult> DefinePositions(int id, List<DefinePositionDto> positions)
    {
        await _groups.DefinePositions(id, positions.Select(p => (p.PositionId, p.Required, p.OpenSpots)));
        return Ok();
    }
}
