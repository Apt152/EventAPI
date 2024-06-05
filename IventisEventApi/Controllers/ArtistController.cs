using IventisEventApi.Database;
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
        public async Task<IEnumerable<Artist>> Get()
        {
            IEnumerable<Artist> allArtists = await _artistService.GetAllArtistsAsync();
            return allArtists;
        }
    }



}
