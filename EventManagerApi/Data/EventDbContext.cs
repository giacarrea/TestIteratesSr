using EventManagerApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventManagerApi.Data
{
    public class EventDbContext : IdentityDbContext
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

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            return await Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<IEnumerable<Event>> GetFilteredAsync(Expression<Func<Event, bool>> filter)
        {
            return await Events.Include(e => e.Registrations).Where(filter).ToListAsync();
        }
        public async Task AddAsync(Event ev)
        {
            await Events.AddAsync(ev);
            await SaveChangesAsync();
        }
        public async Task UpdateAsync(Event ev)
        {
            Events.Update(ev);
            await SaveChangesAsync();
        }
        public async Task DeleteAsync(Event ev)
        {
            Events.Remove(ev);
            await SaveChangesAsync();
        }
    }
}
