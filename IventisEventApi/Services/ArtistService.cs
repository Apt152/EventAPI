using IventisEventApi.Database;
using IventisEventApi.ModelFields;
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

        public async Task<List<Artist>> GetArtistByQueryAsync(ArtistFields fieldName, string query)
        {
            switch (fieldName)
            {
                case ArtistFields.Id:
                    Artist? artist = await GetArtistByIdAsync(Guid.Parse(query));
                    return [artist];
                case ArtistFields.Name:
                    return await GetArtistByNameAsync(query);
                case ArtistFields.Genre:
                    return await GetArtistByGenreAsync(query);
                default:
                    throw new ArgumentException("Invalid field name");
            }
        }

        public async Task<Artist?> GetArtistByIdAsync(Guid artistId)
        {
            return await _context.Artists.FindAsync(artistId);
        }

        public async Task<List<Artist>> GetArtistByNameAsync(string artistName)
        {
            return await _context.Artists.Where(e => e.Name == artistName).ToListAsync();
        }

        public async Task<List<Artist>> GetArtistByGenreAsync(string artistGenre)
        {
            return await _context.Artists.Where(e => e.Genre == artistGenre).ToListAsync();
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
