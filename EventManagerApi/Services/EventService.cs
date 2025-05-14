using EventManagerApi.Data;
using EventManagerApi.Interface;
using EventManagerApi.Models;

namespace EventManagerApi.Services
{
    public class EventService : IEventService
    {
        private readonly EventDbContext _context;
        private readonly ILogger<EventService> _logger; // Add logger

        public EventService(EventDbContext context, ILogger<EventService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Event> CreateEventAsync(Event newEvent)
        {
            newEvent.Id = Guid.NewGuid();
            newEvent.Status = EventStatus.Draft;
            newEvent.Registrations = new List<Registration>();
            await _context.AddAsync(newEvent);
            _logger.LogInformation("Created new event with ID {EventId} and title '{Title}'", newEvent.Id, newEvent.Title);
            return newEvent;
        }

        public async Task<bool> DeleteEventAsync(Guid id)
        {
            var ev = await _context.GetByIdAsync(id);
            if (ev == null)
            {
                _logger.LogWarning("Attempted to delete non-existent event with ID {EventId}", id);
                return false;
            }
            await _context.DeleteAsync(ev);
            _logger.LogInformation("Deleted event with ID {EventId}", id);
            return true;
        }

        
        public async Task<IEnumerable<Event>> GetAllEventsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? category = null,
            EventStatus? status = null) //TODO pagination
        {
            _logger.LogDebug("Retrieving events with filters: startDate={StartDate}, endDate={EndDate}, category={Category}, status={Status}", startDate, endDate, category, status);

            return await _context.GetFilteredAsync(e =>
                (startDate == null || e.Date >= startDate) &&
                (endDate == null || e.Date <= endDate) &&
                (category == null || e.Category == category) &&
                (status == null || e.Status == status)
            );
        }



        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
            var ev = await _context.GetByIdAsync(id);
            if (ev == null)
                _logger.LogWarning("Event with ID {EventId} not found", id);
            else
                _logger.LogDebug("Retrieved event with ID {EventId}", id);
            return ev;
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(Guid eventId)
        {
            var ev = await _context.GetByIdAsync(eventId);
            if (ev == null)
                _logger.LogWarning("Event with ID {EventId} not found when retrieving registrations", eventId);
            else
                _logger.LogDebug("Retrieved registrations for event ID {EventId}", eventId);
            return ev?.Registrations ?? Enumerable.Empty<Registration>();
        }

        public async Task<Registration> RegisterForEventAsync(Guid eventId, string userId) //TODO use jwt identity insdtead of userid
        {
            var ev = await _context.GetByIdAsync(eventId);
            if (ev == null)
            {
                _logger.LogError("Registration failed: Event with ID {EventId} not found", eventId);
                throw new InvalidOperationException("Event not found.");
            }
            if (ev.Registrations.Count >= ev.Capacity)
            {
                _logger.LogWarning("Registration failed: Event with ID {EventId} is full", eventId);
                throw new InvalidOperationException("Event is full.");
            }
            if (ev.Registrations.Any(r => r.UserId == userId))
            {
                _logger.LogWarning("Registration failed: User {UserId} already registered for event {EventId}", userId, eventId);
                throw new InvalidOperationException("User already registered.");
            }

            var registration = new Registration
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                UserId = userId,
                RegisteredAt = DateTime.UtcNow
            };
            ev.Registrations.Add(registration);
            await _context.UpdateAsync(ev);
            _logger.LogInformation("User {UserId} registered for event {EventId}", userId, eventId);
            return registration;
        }

        public async Task<bool> UnregisterFromEventAsync(Guid eventId, string userId) //TODO use jwt identity insdtead of userid
        {
            var ev = await _context.GetByIdAsync(eventId);
            if (ev == null)
            {
                _logger.LogWarning("Unregister failed: Event with ID {EventId} not found", eventId);
                return false;
            }
            var reg = ev.Registrations.FirstOrDefault(r => r.UserId == userId);
            if (reg == null)
            {
                _logger.LogWarning("Unregister failed: User {UserId} not registered for event {EventId}", userId, eventId);
                return false;
            }
            ev.Registrations.Remove(reg);
            await _context.UpdateAsync(ev);
            _logger.LogInformation("User {UserId} unregistered from event {EventId}", userId, eventId);
            return true;
        }

        public async Task<bool> UpdateEventAsync(Guid id, Event updatedEvent)
        {
            var ev = await _context.GetByIdAsync(id);
            if (ev == null)
            {
                _logger.LogWarning("Update failed: Event with ID {EventId} not found", id);
                return false;
            }

            if (ev.Status == EventStatus.Canceled)
            {
                _logger.LogWarning("Update failed: Event with ID {EventId} is canceled and cannot be updated", id);
                return false;
            }

            ev.Title = updatedEvent.Title;
            ev.Description = updatedEvent.Description;
            ev.Date = updatedEvent.Date;
            ev.Location = updatedEvent.Location;
            ev.Category = updatedEvent.Category;
            ev.Capacity = updatedEvent.Capacity;
            ev.Status = updatedEvent.Status;
            await _context.UpdateAsync(ev);
            _logger.LogInformation("Updated event with ID {EventId}", id);
            return true;
        }

    }
}
