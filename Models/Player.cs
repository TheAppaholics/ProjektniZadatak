
namespace FootballStatsAPI.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PlayerStatistics> Statistics { get; set; } = new();
}
