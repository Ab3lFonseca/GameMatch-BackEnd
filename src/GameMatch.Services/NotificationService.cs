using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GameMatch.Services;

public class NotificationService
{
    private readonly AppDb _db;

    public NotificationService(AppDb db)
    {
        _db = db;
    }

    /// <summary>
    /// Cria uma nova notificação para um usuário.
    /// </summary>
    public async Task CreateAsync(int userId, string message, string type = "Info")
    {
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow,
            Read = false
        };

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Retorna todas as notificações de um usuário.
    /// </summary>
    public async Task<List<Notification>> GetByUserAsync(int userId)
    {
        return await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Marca uma notificação como lida.
    /// </summary>
    public async Task MarkAsReadAsync(int notificationId)
    {
        var notif = await _db.Notifications.FindAsync(notificationId);
        if (notif == null) return;

        notif.Read = true;
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Remove notificações antigas (padrão: 30 dias).
    /// </summary>
    public async Task CleanupOldAsync(int days = 30)
    {
        var cutoff = DateTime.UtcNow.AddDays(-days);
        var old = await _db.Notifications
            .Where(n => n.CreatedAt < cutoff)
            .ToListAsync();

        _db.Notifications.RemoveRange(old);
        await _db.SaveChangesAsync();
    }
}
