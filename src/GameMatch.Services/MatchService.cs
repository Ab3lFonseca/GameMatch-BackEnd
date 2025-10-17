using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GameMatch.Services;

public class MatchService
{
    private readonly AppDb _db;
    public MatchService(AppDb db) { _db = db; }

    public async Task<List<User>> SuggestForGroup(int groupId)
    {
        var g = await _db.Groups.Include(x=>x.Positions).ThenInclude(p=>p.Position).FirstAsync(x=>x.Id==groupId);
        var open = g.Positions.Where(p => p.OpenSpots > 0).Select(p => p.PositionId).ToHashSet();
        // Heurística simples: Skills contendo nome da posição (case-insensitive)
        var candidates = await _db.Users.Where(u => u.Skills != null && open.Any(pid =>
            g.Positions.Any(pp => pp.PositionId == pid && (u.Skills!.ToLower().Contains(pp.Position.Name.ToLower()))
        ))).Take(100).ToListAsync();
        return candidates;
    }
}
