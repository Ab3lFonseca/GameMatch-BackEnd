using GameMatch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameMatch.Api.Controllers;

[ApiController]
[Route("api/match")]
//[Authorize]
public class MatchController : ControllerBase
{
    private readonly MatchService _match;
    public MatchController(MatchService match) { _match = match; }

    [HttpGet("{groupId:int}")]
    public async Task<IActionResult> Suggest(int groupId) => Ok(await _match.SuggestForGroup(groupId));
}
