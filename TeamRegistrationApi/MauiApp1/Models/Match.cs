using MauiApp1.Models;

public class Match
{
    public int MatchId { get; set; }
    public int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = new Team();
    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = new Team();
    public DateTime MatchDate { get; set; }
    public string Round { get; set; } = string.Empty;
    public List<PlayerStatistics> PlayerStatistics { get; set; } = new List<PlayerStatistics>();

    // Helper properties for easier binding in XAML
    public string HomeTeamName => HomeTeam?.Name ?? "Unknown";
    public string AwayTeamName => AwayTeam?.Name ?? "Unknown";
}
