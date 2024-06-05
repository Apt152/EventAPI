using IventisEventApi.Database;
using IventisEventApi.ModelFields;
using IventisEventApi.Models;
using IventisEventApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IventisEventApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VenueController(EventDbContext context) : ControllerBase
    {
        private readonly EventDbContext _context = context;

        private readonly VenueService _venueService = new(context);

        [HttpGet(Name = "GetVenues")]
        public async Task<ActionResult<IEnumerable<Venue>>> Get()
        {
            IEnumerable<Venue> allVenues = await _venueService.GetAllVenuesAsync();
            return Ok(allVenues);
        }

        [HttpGet("venuesByField", Name = "GetVenuesByField")]
        public async Task<ActionResult<IEnumerable<Venue>>> GetByField([FromQuery] VenueFields field, [FromQuery] string query)
        {
            try
            {
                IEnumerable<Venue> venues = await _venueService.GetVenueByQueryAsync(field, query);
                if (venues.Any() && venues.First() != null)
                {
                    return Ok(venues);
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
