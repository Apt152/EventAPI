using IventisEventApi.Database;
using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IventisEventApi.Services
{
    public class ArtistService(EventDbContext context)
    {
        private readonly EventDbContext _context = context;

        public async Task<List<Artist>> GetAllArtistsAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task<Artist?> GetArtistByIdAsync(Guid artistId)
        {
            return await _context.Artists.FindAsync(artistId);
        }

        public async Task AddArtistAsync(Artist artist)
        {
            ValidateArtist(artist);
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
        }

        private static void ValidateArtist(Artist artist)
        {
            ArgumentNullException.ThrowIfNull(artist);

            if (!artist.IsComplete())
            {
                throw new ArgumentException("Artist is missing fields and cannnot be added to the database");
            }
        }
    }
}
