using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamRegistrationApi.Models
{
    public class MatchRoundResult
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public Match Match { get; set; } = default!;

        public int RoundNumber { get; set; } // 1, 2, 3, ...

        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}
