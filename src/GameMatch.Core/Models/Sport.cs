using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameMatch.Core.Models;

public class Sport
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
    public string? Description { get; set; }

    public ICollection<Position> Positions { get; set; } = new List<Position>();
    public ICollection<Group> Groups { get; set; } = new List<Group>();
}
