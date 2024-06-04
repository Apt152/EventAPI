using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Services
{
    public class VenueServiceTests
    {
        private readonly DbContextOptions<EventDbContext> _options;
        private readonly EventDbContext _context;
        private readonly VenueService _venueService;

        public VenueServiceTests()
        {
            _options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new EventDbContext(_options);
            _venueService = new VenueService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (!_context.Venues.Any())
            {
                _context.Venues.Add(new Venue { Id = Guid.NewGuid(), Name = "Venue 1", Capacity = 100, BoundingBox = new GeoBoundingBox(53.228317, -0.546745, 53.228295, -0.548888) });
                _context.Venues.Add(new Venue { Id = Guid.NewGuid(), Name = "Venue 2", Capacity = 50, BoundingBox = new GeoBoundingBox(52.912500, -1.183384, 52.909932, -1.186041) });
                _context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetVenueByIdAsync_ReturnsCorrectVenue()
        {
            Venue testVenue = await _context.Venues.FirstAsync();
            Venue? resultVenue = await _venueService.GetVenueById(testVenue.Id);

            Assert.NotNull(resultVenue);
            Assert.Equal(testVenue.Id, resultVenue.Id);
            Assert.Equal(testVenue.Name, resultVenue.Name);
            Assert.Equal(testVenue.Capacity, resultVenue.Capacity);
            Assert.Equal(testVenue.BoundingBox, resultVenue.BoundingBox);
        }

        [Fact]
        public async Task GetVenueByIdAsync_ReturnsNullForMissingVenue()
        {
            Guid randomGuid = Guid.NewGuid();

            Venue? resultVenue = await _venueService.GetVenueById(randomGuid);

            Assert.Null(resultVenue);
        }

        [Fact]
        public async Task AddVenueAsync_AddsVenueSuccessfully()
        {
            Venue newVenue = new() { Id = Guid.NewGuid(), Name = "Venue 3", Capacity = 50, BoundingBox = new GeoBoundingBox(52.953662, -1.150368, 52.953197, -1.149475) };
            await _venueService.AddVenueAsync(newVenue);
            Venue? resultVenue = await _context.Venues.FindAsync(newVenue.Id);

            Assert.NotNull(resultVenue);
            Assert.Equal(newVenue.Id, resultVenue.Id);
            Assert.Equal(newVenue.Name, resultVenue.Name);
            Assert.Equal(newVenue.Capacity, resultVenue.Capacity);
            Assert.Equal(newVenue.BoundingBox, resultVenue.BoundingBox);
        }

        [Fact]
        public async Task AddVenueAsync_DoesNotAddNullVenue()
        {
            Venue? incompleteVenue = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _venueService.AddVenueAsync(incompleteVenue));
        }

        [Fact]
        public async Task AddVenueAsync_DoesNotAddIncompleteVenue()
        {
            Venue incompleteVenue = new() { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => _venueService.AddVenueAsync(incompleteVenue));
            Venue? resultVenue = await _context.Venues.FindAsync(incompleteVenue.Id);

            Assert.Null(resultVenue);
        }
    }
}
