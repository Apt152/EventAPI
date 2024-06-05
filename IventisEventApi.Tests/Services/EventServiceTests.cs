using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace IventisEventApi.Tests.Services
{
    public class EventServiceTests : IAsyncLifetime
    {
        private readonly DbContextOptions<EventDbContext> _options;
        private readonly EventDbContext _context;
        private readonly EventService _EventService;

        public EventServiceTests()
        {
            _options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new EventDbContext(_options);
            _EventService = new EventService(_context);
        }

        public async Task InitializeAsync()
        {
            await SeedDatabaseAsync();
        }

        public Task DisposeAsync()
        {
            _context.Dispose();
            return Task.CompletedTask;
        }

        private async Task SeedDatabaseAsync()
        {
            if (!_context.Venues.Any())
            {
                _context.Venues.AddRange(DummyData.venue1, DummyData.venue2);
                await _context.SaveChangesAsync();
            }
            if (!_context.Events.Any())
            {
                _context.Events.AddRange(DummyData.event1, DummyData.event2);
                await _context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetEventByIdAsync_ReturnsCorrectEvent()
        {
            Event testEvent = await _context.Events.FirstAsync();
            Event? resultEvent = await _EventService.GetEventByIdAsync(testEvent.Id);

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

            Event? resultEvent = await _EventService.GetEventByIdAsync(randomGuid);

            Assert.Null(resultEvent);
        }

        [Fact]
        public async Task AddEventAsync_AddsEventSuccessfully()
        {
            Venue newVenue = DummyData.venue3;
            await _context.Venues.AddAsync(newVenue);
            Event newEvent = DummyData.event3;
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
            Event newEvent = new() { Id = Guid.NewGuid(), Name = "Event 4", Date = new DateOnly(2024, 12, 29), VenueId = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => _EventService.AddEventAsync(newEvent));
        }
    }
}
