using GameMatch.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace GameMatch.Infrastructure;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<GroupPosition> GroupPositions => Set<GroupPosition>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Sport> Sports => Set<Sport>();
    public DbSet<Post> Posts { get; set; } = null!;




    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>().HasIndex(x => x.Email).IsUnique();
        b.Entity<Group>().HasIndex(x => x.Name).IsUnique();
        b.Entity<GroupMember>().HasIndex(x => new { x.GroupId, x.UserId }).IsUnique();
        b.Entity<GroupPosition>().HasIndex(x => new { x.GroupId, x.PositionId }).IsUnique();
    }
}
