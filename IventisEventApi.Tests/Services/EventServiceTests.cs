using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Services
{
    public class EventServiceTests
    {
        private readonly DbContextOptions<EventDbContext> _options;
        private readonly EventDbContext _context;
        private readonly EventService _EventService;

        private readonly Venue _venue1 = new() { Id = Guid.NewGuid(), Name = "Venue 1", Capacity = 100, BoundingBox = new GeoBoundingBox(53.228317, -0.546745, 53.228295, -0.548888) };
        private readonly Venue _venue2 = new() { Id = Guid.NewGuid(), Name = "Venue 2", Capacity = 50, BoundingBox = new GeoBoundingBox(52.912500, -1.183384, 52.909932, -1.186041) };

        public EventServiceTests()
        {
            _options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new EventDbContext(_options);
            _EventService = new EventService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (!_context.Events.Any())
            {   
                _context.Venues.Add(_venue1);
                _context.Venues.Add(_venue2);
                _context.Events.Add(new Event { Id = Guid.NewGuid(), Name = "Event 1", Date = new DateOnly(2024, 6, 4), VenueId = _venue1.Id });
                _context.Events.Add(new Event { Id = Guid.NewGuid(), Name = "Event 2", Date = new DateOnly(2025, 6, 4), VenueId = _venue2.Id });
                _context.Events.Add(new Event { Id = Guid.NewGuid(), Name = "Event 3", Date = new DateOnly(2024, 6, 7), VenueId = _venue1.Id });
                _context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetEventByIdAsync_ReturnsCorrectEvent()
        {
            Event testEvent = await _context.Events.FirstAsync();
            Event? resultEvent = await _EventService.GetEventById(testEvent.Id);

            Assert.NotNull(resultEvent);
            Assert.Equal(testEvent.Id, resultEvent.Id);
            Assert.Equal(testEvent.Name, resultEvent.Name);
            Assert.Equal(testEvent.Date, resultEvent.Date);
            Assert.Equal(testEvent.VenueId, resultEvent.VenueId);
        }

        [Fact]
        public async Task GetEventByIdAsync_ReturnsNullForMissingEvent()
        {
            Guid randomGuid = Guid.NewGuid();

            Event? resultEvent = await _EventService.GetEventById(randomGuid);

            Assert.Null(resultEvent);
        }

        [Fact]
        public async Task AddEventAsync_AddsEventSuccessfully()
        {
            Venue newVenue = new() { Id = Guid.NewGuid(), Name = "Venue 3", Capacity = 50, BoundingBox = new GeoBoundingBox(52.953662, -1.150368, 52.953197, -1.149475) };
            await _context.Venues.AddAsync(newVenue);
            Event newEvent = new() { Id = Guid.NewGuid(), Name = "Event 3", Date = new DateOnly(2024, 12, 29), VenueId = newVenue.Id };
            await _EventService.AddEventAsync(newEvent);
            Event? resultEvent = await _context.Events.FindAsync(newEvent.Id);

            Assert.NotNull(resultEvent);
            Assert.Equal(newEvent.Id, resultEvent.Id);
            Assert.Equal(newEvent.Name, resultEvent.Name);
            Assert.Equal(newEvent.Date, resultEvent.Date);
            Assert.Equal(newEvent.VenueId, resultEvent.VenueId);
        }

        [Fact]
        public async Task AddEventAsync_DoesNotAddNullEvent()
        {
            Event? incompleteEvent = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _EventService.AddEventAsync(incompleteEvent));
        }

        [Fact]
        public async Task AddEventAsync_DoesNotAddIncompleteEvent()
        {
            Event incompleteEvent = new() { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => _EventService.AddEventAsync(incompleteEvent));
            Event? resultEvent = await _context.Events.FindAsync(incompleteEvent.Id);

            Assert.Null(resultEvent);
        }

        [Fact]
        public async Task AddEventAsync_DoesNotAddEventWithUnknownVenue()
        {
            Event newEvent = new() { Id = Guid.NewGuid(), Name = "Event 3", Date = new DateOnly(2024, 12, 29), VenueId = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => _EventService.AddEventAsync(newEvent));
        }
    }
}
