using GameMatch.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameMatch.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    public AuthController(AuthService auth) { _auth = auth; }

    public record RegisterDto(string Name, string Email, string Password);
    public record LoginDto(string Email, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var (ok, msg, token) = await _auth.Register(dto.Name, dto.Email, dto.Password);
        if (!ok) return BadRequest(new { mensagem = msg });
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var (ok, msg, token, user) = await _auth.Login(dto.Email, dto.Password);
        if (!ok) return BadRequest(new { mensagem = msg });
        return Ok(new { token, user = new { user!.Id, user.Name, user.Email } });
    }
}
