using GameMatch.Core.Models;
using GameMatch.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GameMatch.Services;

public class GroupService
{
    private readonly AppDb _db;
    public GroupService(AppDb db) { _db = db; }

    public async Task<Group> Create(string name, string? desc, int sportId, int maxMembers, int ownerId)
    {
        if (!await _db.Sports.AnyAsync(s => s.Id == sportId))
            throw new InvalidOperationException("Esporte inválido");

        if (await _db.Groups.AnyAsync(g => g.Name == name))
            throw new InvalidOperationException("Nome de grupo já existe");

        var g = new Group { Name = name, Description = desc, MaxMembers = maxMembers, OwnerId = ownerId, SportId = sportId };
        _db.Groups.Add(g);
        await _db.SaveChangesAsync();

        _db.GroupMembers.Add(new GroupMember { GroupId = g.Id, UserId = ownerId, Role = MemberRole.Owner });
        await _db.SaveChangesAsync();
        return g;
    }


    public async Task<List<Group>> List() =>
        await _db.Groups.Include(x => x.Members).ThenInclude(m => m.User)
                        .Include(x => x.Positions).ThenInclude(p => p.Position).ToListAsync();

    public async Task DefinePositions(int groupId, IEnumerable<(int positionId, int required, int open)> items)
    {
        var g = await _db.Groups.Include(x=>x.Positions).FirstOrDefaultAsync(x=>x.Id==groupId) ?? throw new KeyNotFoundException();
        g.Positions.Clear();
        foreach (var (pid, req, open) in items)
            g.Positions.Add(new GroupPosition { GroupId = groupId, PositionId = pid, Required = req, OpenSpots = open });
        await _db.SaveChangesAsync();
    }

    public async Task Join(int groupId, int userId)
    {
        if (await _db.GroupMembers.AnyAsync(m => m.GroupId == groupId && m.UserId == userId))
            throw new InvalidOperationException("Já é membro");
        var g = await _db.Groups.FindAsync(groupId) ?? throw new KeyNotFoundException();
        var count = await _db.GroupMembers.CountAsync(m => m.GroupId == groupId);
        if (count >= g.MaxMembers) throw new InvalidOperationException("Grupo lotado");
        _db.GroupMembers.Add(new GroupMember { GroupId = groupId, UserId = userId, Role = MemberRole.Member });
        await _db.SaveChangesAsync();
    }
}
