using EventManagerApi.Interface;
using EventManagerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManagerApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/Event
        [HttpGet]
        [Authorize] // Any authenticated user can view events
        public async Task<ActionResult<IEnumerable<Event>>> Get(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? category = null,
            [FromQuery] EventStatus? status = null)
        {
            try
            {
                var events = await _eventService.GetAllEventsAsync(startDate, endDate, category, status);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/Event
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Event>> Post([FromBody] Event newEvent)
        {
            try
            {
                var created = await _eventService.CreateEventAsync(newEvent);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/Event/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Event updatedEvent)
        {
            try
            {
                var result = await _eventService.UpdateEventAsync(id, updatedEvent);
                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/Event/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _eventService.DeleteEventAsync(id);
                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Event/{id}/registrations
        [HttpGet("{id}/registrations")]
        [Authorize] // Any authenticated user can view registrations
        public async Task<ActionResult<IEnumerable<Registration>>> GetRegistrations(Guid id)
        {
            try
            {
                var registrations = await _eventService.GetRegistrationsByEventIdAsync(id);
                return Ok(registrations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/Event/{id}/registrations
        [HttpPost("{id}/registrations")]
        [Authorize] // Any authenticated user can register
        public async Task<ActionResult<Registration>> Register(Guid id)
        {
            try
            {
                var userId = User?.Identity?.Name;
                if (string.IsNullOrEmpty(userId))
                    return BadRequest(new { error = "User ID not found in token." });

                var registration = await _eventService.RegisterForEventAsync(id, userId);
                return CreatedAtAction(nameof(GetRegistrations), new { id }, registration);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/Event/{id}/registrations
        [HttpDelete("{id}/registrations")]
        [Authorize] // Any authenticated user can unregister
        public async Task<IActionResult> Unregister(Guid id)
        {
            try
            {
                var userId = User?.Identity?.Name;
                if (string.IsNullOrEmpty(userId))
                    return BadRequest(new { error = "User ID not found in token." });

                var result = await _eventService.UnregisterFromEventAsync(id, userId);
                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
