using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using IventisEventApi.Tests.Database;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Services
{
    public class VenueServiceTests
    {
        private VenueDatabaseSeeding _venueDbSeeding;

        public VenueServiceTests()
        {
            _venueDbSeeding = new();
        }

        [Fact]
        public async Task GetVenueByIdAsync_ReturnsCorrectVenue()
        {
            using EventDbContext dbContext = await _venueDbSeeding.CreateNewDatabase();
            VenueService venueService = new(dbContext);

            Venue testVenue = await dbContext.Venues.FirstAsync();
            Venue? resultVenue = await venueService.GetVenueByIdAsync(testVenue.Id);

            Assert.NotNull(resultVenue);
            Assert.Equal(testVenue.Id, resultVenue.Id);
            Assert.Equal(testVenue.Name, resultVenue.Name);
            Assert.Equal(testVenue.Capacity, resultVenue.Capacity);
            Assert.Equal(testVenue.BoundingBox, resultVenue.BoundingBox);
        }

        [Fact]
        public async Task GetVenueByIdAsync_ReturnsNullForMissingVenue()
        {
            using EventDbContext dbContext = await _venueDbSeeding.CreateNewDatabase();
            VenueService venueService = new(dbContext);

            Guid randomGuid = Guid.NewGuid();

            Venue? resultVenue = await venueService.GetVenueByIdAsync(randomGuid);

            Assert.Null(resultVenue);
        }

        [Fact]
        public async Task AddVenueAsync_AddsVenueSuccessfully()
        {
            using EventDbContext dbContext = await _venueDbSeeding.CreateNewDatabase();
            VenueService venueService = new(dbContext);

            Venue newVenue = new() { Id = Guid.NewGuid(), Name = "Venue 3", Capacity = 50, BoundingBox = new GeoBoundingBox(52.953662, -1.150368, 52.953197, -1.149475) };
            await venueService.AddVenueAsync(newVenue);
            Venue? resultVenue = await dbContext.Venues.FindAsync(newVenue.Id);

            Assert.NotNull(resultVenue);
            Assert.Equal(newVenue.Id, resultVenue.Id);
            Assert.Equal(newVenue.Name, resultVenue.Name);
            Assert.Equal(newVenue.Capacity, resultVenue.Capacity);
            Assert.Equal(newVenue.BoundingBox, resultVenue.BoundingBox);
        }

        [Fact]
        public async Task AddVenueAsync_DoesNotAddNullVenue()
        {
            using EventDbContext dbContext = await _venueDbSeeding.CreateNewDatabase();
            VenueService venueService = new(dbContext);

            Venue? incompleteVenue = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => venueService.AddVenueAsync(incompleteVenue));
        }

        [Fact]
        public async Task AddVenueAsync_DoesNotAddIncompleteVenue()
        {
            using EventDbContext dbContext = await _venueDbSeeding.CreateNewDatabase();
            VenueService venueService = new(dbContext);

            Venue incompleteVenue = new() { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => venueService.AddVenueAsync(incompleteVenue));
            Venue? resultVenue = await dbContext.Venues.FindAsync(incompleteVenue.Id);

            Assert.Null(resultVenue);
        }
    }
}
