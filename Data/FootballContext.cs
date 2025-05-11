
using Microsoft.EntityFrameworkCore;
using FootballStatsAPI.Models;

namespace FootballStatsAPI.Data;

public class FootballContext : DbContext
{
    public FootballContext(DbContextOptions<FootballContext> options) : base(options) { }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<PlayerStatistics> PlayerStatistics => Set<PlayerStatistics>();
}
