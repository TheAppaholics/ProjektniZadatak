using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeamRegistrationApi.Models;

namespace TeamRegistrationApi.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        [ForeignKey("HomeTeam")]
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; } = default!;

        [ForeignKey("AwayTeam")]
        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; } = default!;

        public DateTime MatchDate { get; set; }

        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string Round { get; set; } = string.Empty;


        // Dodano:
        public List<PlayerStatistics> PlayerStatistics { get; set; } = new();

        public List<MatchRoundResult> RoundResults { get; set; } = new();

    }
}
