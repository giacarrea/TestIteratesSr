using EventManagerApi.Models;

namespace EventManagerApi.Interface
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? category = null,
            EventStatus? status = null);
        Task<Event?> GetEventByIdAsync(Guid id);
        Task<Event> CreateEventAsync(Event newEvent);
        Task<bool> UpdateEventAsync(Guid id, Event updatedEvent);
        Task<bool> DeleteEventAsync(Guid id);
        Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(Guid eventId);
        Task<Registration> RegisterForEventAsync(Guid eventId, string userId);
        Task<bool> UnregisterFromEventAsync(Guid eventId, string userId);
    }
}
