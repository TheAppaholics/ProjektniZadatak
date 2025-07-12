namespace TeamRegistrationApi.Models
{
    public class MatchDto
    {
        public int MatchId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public DateTime MatchDate { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string Round { get; set; } = string.Empty;
        public List<MatchRoundResultDto> RoundResults { get; set; } = new();

    }
}
