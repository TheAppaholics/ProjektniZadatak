namespace TeamRegistrationApi.Models
{
    public class Team
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string TournamentName { get; set; }
        public DateTime TournamentDate { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();
    }
}
