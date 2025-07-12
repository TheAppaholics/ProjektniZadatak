using MauiApp1.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;  // Inicijaliziraj da ne bude null
    public string TournamentName { get; set; } = string.Empty;
    public DateTime TournamentDate { get; set; }

    public List<Player> Players { get; set; } = new List<Player>();  // Inicijaliziraj listu da ne bude null
}
