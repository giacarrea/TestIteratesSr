using EventManagerApi.Models;
using System.Linq.Expressions;

namespace EventManagerApi.Interface
{
    public interface IEventDbContext
    {
        Task<Event?> GetByIdAsync(Guid id);
        Task<IEnumerable<Event>> GetFilteredAsync(Expression<Func<Event, bool>> filter);
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task AddRegistrationAsync(Event ev, Registration reg);
        Task DeleteAsync(Event ev);
    }
}
