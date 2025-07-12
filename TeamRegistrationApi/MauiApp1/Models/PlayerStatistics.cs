using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class PlayerStatistics
    {
        public int Id { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }

        public int PlayerId { get; set; }
        public int MatchId { get; set; }
        // Možeš dodati i ime utakmice ako ga backend vraća kao string
        // public string MatchName { get; set; }
    }
}
