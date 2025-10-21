using System;

namespace GameMatch.Core.Models;

public class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = default!;  // ← ESSA LINHA É ESSENCIAL

    public string Type { get; set; } = "Info";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool Read { get; set; } = false;

    public User? User { get; set; }
}
