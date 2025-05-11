using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace TeamRegistrationApi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }
        public int TeamId { get; set; }

        [JsonIgnore]
        [BindNever]
        public Team? Team { get; set; }

        // Dodaj tvoj dio:
        public List<PlayerStatistics> Statistics { get; set; } = new();
    }
}
