using Microsoft.EntityFrameworkCore;

namespace LoyaltyServiceLab2.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Loyalty> Loyalties { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string host = Environment.GetEnvironmentVariable("DBHOST");
            string port = Environment.GetEnvironmentVariable("DBPORT");
            string db = Environment.GetEnvironmentVariable("DATABASE");
            string user = Environment.GetEnvironmentVariable("USERNAME");
            string password = Environment.GetEnvironmentVariable("PASSWORD");
            optionsBuilder.UseNpgsql($"Host={host};Port={port};Database={db};Username={user};Password={password}");
        }
    }
}
