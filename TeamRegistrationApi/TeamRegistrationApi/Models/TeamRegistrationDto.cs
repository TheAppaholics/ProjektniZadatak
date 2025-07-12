namespace TeamRegistrationApi.Models
{
    public class TeamRegistrationDto
    {
        public int Id { get; set; } // ✅ Dodaj ovo

        public required string Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
    }

}
