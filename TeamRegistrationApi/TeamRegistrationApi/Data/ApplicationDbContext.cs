using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TeamRegistrationApi.Models;

namespace TeamRegistrationApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
