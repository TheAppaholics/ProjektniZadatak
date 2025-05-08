namespace LeaderboardAPI.Models
{
    public class MatchResult
    {
        public int Id { get; set; }

        public string TeamA { get; set; }
        public string TeamB { get; set; }

        public int ScoreA { get; set; }
        public int ScoreB { get; set; }

        public DateTime MatchDate { get; set; }
    }
}
