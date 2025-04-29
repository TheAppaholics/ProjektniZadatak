namespace TeamRegistrationApi.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TournamentName { get; set; }
        public DateTime TournamentDate { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();
    }
}
