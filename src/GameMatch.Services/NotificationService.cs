using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GameMatch.Services;

public class NotificationService
{
    private readonly AppDb _db;
    public NotificationService(AppDb db) { _db = db; }

    public async Task NotifySpotOpen(int groupId, int positionId)
    {
        var pos = await _db.Positions.FindAsync(positionId);
        if (pos == null) return;
        var users = await _db.Users.Where(u => u.Skills != null && u.Skills.ToLower().Contains(pos.Name.ToLower())).ToListAsync();
        foreach (var u in users)
        {
            _db.Notifications.Add(new Notification { UserId = u.Id, Type = "spot-open", Payload = $"{{"groupId":{groupId},"positionId":{positionId}}}" });
        }
        await _db.SaveChangesAsync();
    }
}
