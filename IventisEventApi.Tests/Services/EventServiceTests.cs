using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using IventisEventApi.Tests.Database;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Services
{
    public class EventServiceTests
    {
        private EventDatabaseSeeding _eventDbSeeding;

        public EventServiceTests()
        {
            _eventDbSeeding = new();
        }

        [Fact]
        public async Task GetEventByIdAsync_ReturnsCorrectEvent()
        {
            using EventDbContext dbContext = await _eventDbSeeding.CreateNewDatabase();
            EventService eventService = new(dbContext);

            Event testEvent = await dbContext.Events.FirstAsync();
            Event? resultEvent = await eventService.GetEventByIdAsync(testEvent.Id);

            Assert.NotNull(resultEvent);
            Assert.Equal(testEvent.Id, resultEvent.Id);
            Assert.Equal(testEvent.Name, resultEvent.Name);
            Assert.Equal(testEvent.Date, resultEvent.Date);
            Assert.Equal(testEvent.VenueId, resultEvent.VenueId);
        }

        [Fact]
        public async Task GetEventByIdAsync_ReturnsNullForMissingEvent()
        {
            using EventDbContext dbContext = await _eventDbSeeding.CreateNewDatabase();
            EventService eventService = new(dbContext);

            Guid randomGuid = Guid.NewGuid();

            Event? resultEvent = await eventService.GetEventByIdAsync(randomGuid);

            Assert.Null(resultEvent);
        }

        [Fact]
        public async Task AddEventAsync_AddsEventSuccessfully()
        {
            using EventDbContext dbContext = await _eventDbSeeding.CreateNewDatabase();
            EventService eventService = new(dbContext);

            Venue newVenue = new() { Id = Guid.NewGuid(), Name = "Test Venue", Capacity = 50, BoundingBox = new GeoBoundingBox(52.953662, -1.150368, 52.953197, -1.149475) };
            await dbContext.Venues.AddAsync(newVenue);
            Event newEvent = new() { Id = Guid.NewGuid(), Name = "Test Event", Date = new DateOnly(2024, 6, 4), VenueId = newVenue.Id }; ;
            await eventService.AddEventAsync(newEvent);
            Event? resultEvent = await dbContext.Events.FindAsync(newEvent.Id);

            Assert.NotNull(resultEvent);
            Assert.Equal(newEvent.Id, resultEvent.Id);
            Assert.Equal(newEvent.Name, resultEvent.Name);
            Assert.Equal(newEvent.Date, resultEvent.Date);
            Assert.Equal(newEvent.VenueId, resultEvent.VenueId);
        }

        [Fact]
        public async Task AddEventAsync_DoesNotAddNullEvent()
        {
            using EventDbContext dbContext = await _eventDbSeeding.CreateNewDatabase();
            EventService eventService = new(dbContext);

            Event? incompleteEvent = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => eventService.AddEventAsync(incompleteEvent));
        }

        [Fact]
        public async Task AddEventAsync_DoesNotAddIncompleteEvent()
        {
            using EventDbContext dbContext = await _eventDbSeeding.CreateNewDatabase();
            EventService eventService = new(dbContext);

            Event incompleteEvent = new() { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => eventService.AddEventAsync(incompleteEvent));
            Event? resultEvent = await dbContext.Events.FindAsync(incompleteEvent.Id);

            Assert.Null(resultEvent);
        }

        [Fact]
        public async Task AddEventAsync_DoesNotAddEventWithUnknownVenue()
        {
            using EventDbContext dbContext = await _eventDbSeeding.CreateNewDatabase();
            EventService eventService = new(dbContext);

            Event newEvent = new() { Id = Guid.NewGuid(), Name = "Event 4", Date = new DateOnly(2024, 12, 29), VenueId = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => eventService.AddEventAsync(newEvent));
        }
    }
}
