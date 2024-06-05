using IventisEventApi.Database;
using IventisEventApi.ModelFields;
using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IventisEventApi.Services
{
    public class VenueService(EventDbContext context)
    {
        private readonly EventDbContext _context = context;

        public async Task<List<Venue>> GetAllVenuesAsync()
        {
            return await _context.Venues.ToListAsync();
        }

        public async Task<List<Venue>> GetVenueByQueryAsync(VenueFields fieldName, string query)
        {
            switch (fieldName)
            {
                case VenueFields.Id:
                    Venue? venue = await GetVenueByIdAsync(Guid.Parse(query));
                    return [venue];
                case VenueFields.Name:
                    return await GetVenueByNameAsync(query);
                default:
                    throw new ArgumentException("Invalid field name");
            }
        }

        public async Task<Venue?> GetVenueByIdAsync(Guid venueId)
        {
            return await _context.Venues.FindAsync(venueId);
        }

        public async Task<List<Venue>> GetVenueByNameAsync(string venueName)
        {
            return await _context.Venues.Where(e => e.Name == venueName).ToListAsync();
        }

        public async Task AddVenueAsync(Venue venue)
        {
            ValidateVenue(venue);
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
        }

        private static void ValidateVenue(Venue venue)
        {
            ArgumentNullException.ThrowIfNull(venue);

            if (!venue.IsComplete())
            {
                throw new ArgumentException("Venue is missing fields and cannnot be added to the database");
            }
        }
    }
}
