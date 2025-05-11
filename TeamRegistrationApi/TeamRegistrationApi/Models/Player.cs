using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace TeamRegistrationApi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }
        public int TeamId { get; set; }

        [JsonIgnore]
        [BindNever]                // ne očekuj ni u model bindingu
        public Team? Team { get; set; }
    }
}