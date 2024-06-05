using IventisEventApi.Controllers;
using IventisEventApi.Database;
using IventisEventApi.ModelFields;
using IventisEventApi.Models;
using IventisEventApi.Tests.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Controllers
{
    public class VenueControllerTests : IAsyncLifetime
    {
        private readonly DbContextOptions<EventDbContext> _options;
        private readonly EventDbContext _context;
        private readonly VenueController _venueController;

        public VenueControllerTests()
        {
            _options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new EventDbContext(_options);
            _venueController = new VenueController(_context);
        }

        public async Task InitializeAsync()
        {
            await VenueDatabaseSeeding.SeedWithDefaultVenues(_context);
        }

        public Task DisposeAsync()
        {
            _context.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Get_ReturnsCorrectListOfVenues()
        {
            IEnumerable<Venue> expectedVenues = await _context.Venues.ToListAsync();
            ActionResult<IEnumerable<Venue>> result = await _venueController.Get();
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);

            IEnumerable<Venue> actualVenues = Assert.IsAssignableFrom<IEnumerable<Venue>>(okResult.Value);

            Assert.NotNull(actualVenues);
            Assert.Equal(expectedVenues.Count(), actualVenues.Count());
            Assert.Equal(expectedVenues.First().Id, actualVenues.First().Id);
            Assert.Equal(expectedVenues.Last().Id, actualVenues.Last().Id);
        }

        [Fact]
        public async Task Get_ReturnsEmptyListWhenNoVenues()
        {
            await VenueDatabaseSeeding.ClearVenueTableAsync(_context);

            ActionResult<IEnumerable<Venue>> result = await _venueController.Get();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            IEnumerable<Venue> venues = Assert.IsAssignableFrom<IEnumerable<Venue>>(okResult.Value);

            Assert.NotNull(venues);
            Assert.Empty(venues);

            await VenueDatabaseSeeding.SeedWithDefaultVenues(_context);
        }

        [Fact]
        public async Task Get_ReturnsCorrectListOfVenuesWhenManyVenues()
        {
            int numberOfVenues = 100;
            await VenueDatabaseSeeding.ClearVenueTableAsync(_context);
            await VenueDatabaseSeeding.CreateManyVenueEntries(_context, numberOfVenues);

            IEnumerable<Venue> expectedVenues = await _context.Venues.ToListAsync();

            ActionResult<IEnumerable<Venue>> result = await _venueController.Get();
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);

            IEnumerable<Venue> actualVenues = Assert.IsAssignableFrom<IEnumerable<Venue>>(okResult.Value);

            Assert.NotNull(actualVenues);
            Assert.Equal(numberOfVenues, actualVenues.Count());
            Assert.Equal(expectedVenues.First().Id, actualVenues.First().Id);
            Assert.Equal(expectedVenues.Last().Id, actualVenues.Last().Id);
        }

        [Fact]
        public async Task GetByField_ReturnsBadRequestOnArgumentException()
        {
            ActionResult<IEnumerable<Venue>> result = await _venueController.GetByField(VenueFields.Invalid, "Test");
            Assert.IsType<BadRequestObjectResult>(result.Result);

            result = await _venueController.GetByField(VenueFields.BoundingBox, DummyData.venue1.BoundingBox.ToString());
            Assert.IsType<BadRequestObjectResult>(result.Result);

            result = await _venueController.GetByField(VenueFields.Capacity, 1.ToString());
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByField_ReturnsOkOnValidRequest()
        {
            ActionResult<IEnumerable<Venue>> result = await _venueController.GetByField(VenueFields.Id, DummyData.venue1.Id.ToString());
            Assert.IsType<OkObjectResult>(result.Result);

            result = await _venueController.GetByField(VenueFields.Name, DummyData.venue1.Name);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByField_ReturnsNotFoundOnEmptyRequestReturn()
        {
            ActionResult<IEnumerable<Venue>> result = await _venueController.GetByField(VenueFields.Id, Guid.NewGuid().ToString());
            Assert.IsType<NotFoundResult>(result.Result);

            result = await _venueController.GetByField(VenueFields.Name, "");
            Assert.IsType<NotFoundResult>(result.Result);
        }

    }
}
