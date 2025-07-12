using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
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
