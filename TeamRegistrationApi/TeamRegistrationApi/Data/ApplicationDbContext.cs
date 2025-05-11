using Microsoft.EntityFrameworkCore;
using TeamRegistrationApi.Models;  // Dodaj modele iz glavnog projekta

namespace TeamRegistrationApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSet-ovi iz glavnog projekta
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; } // ✅ dodano

        // DbSet-ovi iz projekta (FootballStatsAPI)
        public DbSet<TeamRegistrationApi.Models.Match> FootballMatches { get; set; }
        public DbSet<TeamRegistrationApi.Models.Player> FootballPlayers { get; set; }
        public DbSet<TeamRegistrationApi.Models.PlayerStatistics> FootballPlayerStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
