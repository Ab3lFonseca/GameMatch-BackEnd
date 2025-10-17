using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GameMatch.Services;

public class JwtOptions
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpiresMinutes { get; set; } = 240;
}

public class AuthService
{
    private readonly AppDb _db;
    private readonly JwtOptions _jwt;

    public AuthService(AppDb db, JwtOptions opts)
    {
        _db = db;
        _jwt = opts;
    }

    public async Task<(bool ok, string msg, string? token)> Register(string name, string email, string password)
    {
        if (await _db.Users.AnyAsync(u => u.Email == email))
            return (false, "Email já cadastrado", null);
        var user = new User { Name = name, Email = email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        var token = GenerateToken(user);
        return (true, "ok", token);
    }

    public async Task<(bool ok, string msg, string? token, User? user)> Login(string email, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return (false, "Usuário ou senha inválidos", null, null);
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return (false, "Usuário ou senha inválidos", null, null);
        var token = GenerateToken(user);
        return (true, "ok", token, user);
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name)
        };
        var token = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims, expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiresMinutes), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
