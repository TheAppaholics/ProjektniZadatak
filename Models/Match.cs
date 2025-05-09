
namespace FootballStatsAPI.Models;

public class Match
{
    public int Id { get; set; }
    public string TeamA { get; set; } = string.Empty;
    public string TeamB { get; set; } = string.Empty;
    public List<PlayerStatistics> PlayerStatistics { get; set; } = new();
}
