using FootballStatsAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using FootballStatsAPI; // zamijeni sa stvarnim namespace-om

namespace FootballStatsAPI.Data // zamijeni sa stvarnim namespace-om
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dodatna konfiguracija ako trebaš, npr. relacije
        }
    }
}
