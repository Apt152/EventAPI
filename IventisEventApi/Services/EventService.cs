using IventisEventApi.Database;
using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IventisEventApi.Services
{
    public class EventService(EventDbContext context)
    {
        private readonly EventDbContext _context = context;

        public async Task<Event?> GetEventById(Guid eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }

        public async Task AddEventAsync(Event newEvent)
        {
            await ValidateEvent(newEvent);

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
        }

        private async Task ValidateEvent(Event newEvent)
        {
            ArgumentNullException.ThrowIfNull(newEvent);

            if (!newEvent.IsComplete())
            {
                throw new ArgumentException("Event is missing fields and cannnot be added to the database");
            }

            _ = await _context.Venues.FindAsync(newEvent.VenueId) ?? throw new ArgumentException("Venue does not exist");

        }
    }
}
