using EventManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerApi.Data
{
    public class EventDbContext : DbContext //TODO: dériver de IdentityDbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EventManagerDb;Trusted_Connection=True;");
            }
        }
    }
}
