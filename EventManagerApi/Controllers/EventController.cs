using EventManagerApi.Interface;
using EventManagerApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManagerApi.Controllers
{
    //TODO restrict jwt roles

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
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Event>>> Get()
        //{
        //    try
        //    {
        //        var events = await _eventService.GetAllEventsAsync();
        //        return Ok(events);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = ex.Message });
        //    }
        //}

        // GET: api/Event
        [HttpGet]
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
        public async Task<ActionResult<Registration>> Register(Guid id, [FromBody] string userId)
        {
            try
            {
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

        // DELETE: api/Event/{id}/registrations/{userId}
        [HttpDelete("{id}/registrations/{userId}")]
        public async Task<IActionResult> Unregister(Guid id, string userId)
        {
            try
            {
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
