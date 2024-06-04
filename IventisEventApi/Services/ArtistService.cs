using IventisEventApi.Database;
using IventisEventApi.Models;

namespace IventisEventApi.Services
{
    public class ArtistService(EventDbContext context)
    {
        private readonly EventDbContext _context = context;

        public async Task<Artist?> GetArtistById(Guid artistId)
        {
            return await _context.Artists.FindAsync(artistId);
        }

        public async Task AddArtistAsync(Artist artist)
        {
            ArgumentNullException.ThrowIfNull(artist);

            if (!artist.IsComplete())
            {
                throw new ArgumentException("Artist is missing fields and cannnot be added to the database");
            }
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
        }
    }
}
