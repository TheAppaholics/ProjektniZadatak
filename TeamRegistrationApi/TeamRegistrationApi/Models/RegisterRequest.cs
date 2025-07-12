namespace TeamRegistrationApi.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        // Možeš dodati i ostale podatke ako želiš, npr. Ime, Prezime itd.
    }
}
