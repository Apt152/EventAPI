using IventisEventApi.Database;
using IventisEventApi.Models;

namespace IventisEventApi.Services
{
    public class VenueService(EventDbContext context)
    {
        private readonly EventDbContext _context = context;

        public async Task<Venue?> GetVenueById(Guid venueId)
        {
            return await _context.Venues.FindAsync(venueId);
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
