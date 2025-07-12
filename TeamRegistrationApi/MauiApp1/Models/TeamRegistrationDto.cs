using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class TeamRegistrationDto
    {
        public int Id { get; set; } // ✅ Dodaj ovo

        public string Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
    }

}
