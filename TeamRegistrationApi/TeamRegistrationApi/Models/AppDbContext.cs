using Microsoft.EntityFrameworkCore;

namespace LeaderboardAPI.Models
{
    // Kreiram klasu AppDbContext koja nasljeđuje DbContext iz Entity Framework-a
    public class AppDbContext : DbContext
    {
        // Konstruktor koji prosljeđuje DbContextOptions bazičnoj klasi (DbContext)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Definišem DbSet za tabelu MatchResults, ovo će omogućiti rad sa tom tabelom u bazi
        public DbSet<MatchResult> MatchResults { get; set; }
    }
}
