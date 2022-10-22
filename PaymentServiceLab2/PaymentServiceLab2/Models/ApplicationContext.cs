using Microsoft.EntityFrameworkCore;

namespace PaymentServiceLab2.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

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
            Console.WriteLine($"Host={host};Port={port};Database={db};Username={user};Password={password}");
            optionsBuilder.UseNpgsql($"Host={host};Port={port};Database={db};Username={user};Password={password}");
        }
    }
}
