using IventisEventApi.Database;
using IventisEventApi.ModelFields;
using IventisEventApi.Models;
using IventisEventApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IventisEventApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController(EventDbContext context) : ControllerBase
    {
        private readonly EventDbContext _context = context;

        private readonly EventService _eventObjService = new(context);

        [HttpGet(Name = "GetEvents")]
        public async Task<ActionResult<IEnumerable<Event>>> Get()
        {
            IEnumerable<Event> allEvents = await _eventObjService.GetAllEventsAsync();
            return Ok(allEvents);
        }

        [HttpGet("eventsByField", Name = "GetEventsByField")]
        public async Task<ActionResult<IEnumerable<Event>>> GetByField([FromQuery] EventFields field, [FromQuery] string query)
        {
            try
            {
                IEnumerable<Event> events = await _eventObjService.GetEventByQueryAsync(field, query);
                if (events.Any() && events.First() != null)
                {
                    return Ok(events);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }



}
