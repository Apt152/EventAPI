using IventisEventApi.Database;
using IventisEventApi.ModelFields;
using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IventisEventApi.Services
{
    public class EventService(EventDbContext context)
    {
        private readonly EventDbContext _context = context;

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<List<Event>> GetEventByQueryAsync(EventFields fieldName, string query)
        {
            switch (fieldName)
            {
                case EventFields.Id:
                    Event? eventObj = await GetEventByIdAsync(Guid.Parse(query));
                    return [eventObj];
                case EventFields.Name:
                    return await GetEventByNameAsync(query);
                case EventFields.Date:
                    return await GetEventByDateAsync(DateOnly.Parse(query));
                case EventFields.VenueId:
                    return await GetEventByVenueIdAsync(Guid.Parse(query));
                default:
                    throw new ArgumentException("Invalid field name");
            }
        }

        public async Task<Event?> GetEventByIdAsync(Guid eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }

        public async Task<List<Event>> GetEventByNameAsync(string eventName)
        {
            return await _context.Events.Where(e => e.Name == eventName).ToListAsync();
        }

        public async Task<List<Event>> GetEventByDateAsync(DateOnly eventDate)
        {
            return await _context.Events.Where(e => e.Date == eventDate).ToListAsync();
        }

        public async Task<List<Event>> GetEventByVenueIdAsync(Guid venueId)
        {
            return await _context.Events.Where(e => e.VenueId == venueId).ToListAsync();
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
