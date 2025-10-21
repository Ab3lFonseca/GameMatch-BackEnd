using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using GameMatch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameMatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupsController(GroupService groupService)
    {
        _groupService = groupService;
    }

    // DTO usado para criar grupos
    public class CreateGroupDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int SportId { get; set; }         // ✅ adicionamos o campo SportId
        public int MaxMembers { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto dto)
    {
        // ✅ Certifique-se de não ter duas variáveis com o mesmo nome
        var ownerId = 1; // substitua por ID do usuário autenticado quando JWT estiver pronto

        var group = await _groupService.Create(
            dto.Name,
            dto.Description,
            dto.SportId,
            dto.MaxMembers,
            ownerId
        );

        return Created($"/api/groups/{group.Id}", group);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromServices] AppDb db)
    {
        var groups = await db.Groups.Include(g => g.Sport).ToListAsync();
        return Ok(groups);
    }
}
