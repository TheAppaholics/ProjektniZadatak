
namespace FootballStatsAPI.Models;

public class PlayerStatistics
{
    public int Id { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public int MatchId { get; set; }
    public Match Match { get; set; } = null!;
}
