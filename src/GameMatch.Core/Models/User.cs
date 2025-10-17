namespace GameMatch.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string? Phone { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Skills { get; set; } // texto livre
    public string? Availability { get; set; }
}
