using IventisEventApi.Database;
using IventisEventApi.ModelFields;
using IventisEventApi.Models;
using IventisEventApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IventisEventApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistController(EventDbContext context) : ControllerBase
    {
        private readonly EventDbContext _context = context;

        private readonly ArtistService _artistService = new(context);



        [HttpGet(Name = "GetArtists")]
        public async Task<ActionResult<IEnumerable<Artist>>> Get()
        {
            IEnumerable<Artist> allArtists = await _artistService.GetAllArtistsAsync();
            return Ok(allArtists);
        }

        [HttpGet("artistsByField", Name = "GetArtistsByField")]
        public async Task<ActionResult<IEnumerable<Artist>>> GetByField([FromQuery] ArtistFields field, [FromQuery] string query)
        {
            try
            {
                IEnumerable<Artist> artists = await _artistService.GetArtistByQueryAsync(field, query);
                if (artists.Any() && artists.First() != null)
                {
                    return Ok(artists);
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
