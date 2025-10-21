namespace GameMatch.Core.Models;

public class Group
{
    public int Id { get; set; }
    public int SportId { get; set; }
    public Sport Sport { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? MeetingAddress { get; set; }
    public DateTime? MeetingDate { get; set; }
    public string? PhotoUrl { get; set; }
    public int MaxMembers { get; set; } = 10;
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    public ICollection<GroupPosition> Positions { get; set; } = new List<GroupPosition>();
}
public class Position
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Characteristics { get; set; }
    public int SportId { get; set; }
    public Sport Sport { get; set; } = default!;
}

public class GroupPosition
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;
    public int PositionId { get; set; }
    public Position Position { get; set; } = default!;
    public int Required { get; set; } = 0;
    public int OpenSpots { get; set; } = 0;
}
public class GroupMember
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public int? PositionId { get; set; }
    public Position? Position { get; set; }
    public MemberRole Role { get; set; } = MemberRole.Member;
}
public class Event
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;
    public DateTime StartAt { get; set; }
    public string? Place { get; set; }
}
