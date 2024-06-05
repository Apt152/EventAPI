using IventisEventApi.Controllers;
using IventisEventApi.Database;
using IventisEventApi.ModelFields;
using IventisEventApi.Models;
using IventisEventApi.Tests.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Controllers
{
    public class EventControllerTests : IAsyncLifetime
    {
        private readonly DbContextOptions<EventDbContext> _options;
        private readonly EventDbContext _context;
        private readonly EventController _eventController;

        public EventControllerTests()
        {
            _options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new EventDbContext(_options);
            _eventController = new EventController(_context);
        }

        public async Task InitializeAsync()
        {
            await EventDatabaseSeeding.SeedWithDefaultEvents(_context);
        }

        public Task DisposeAsync()
        {
            _context.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Get_ReturnsCorrectListOfEvents()
        {
            IEnumerable<Event> expectedEvents = await _context.Events.ToListAsync();
            ActionResult<IEnumerable<Event>> result = await _eventController.Get();
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);

            IEnumerable<Event> actualEvents = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);

            Assert.NotNull(actualEvents);
            Assert.Equal(expectedEvents.Count(), actualEvents.Count());
            Assert.Equal(expectedEvents.First().Id, actualEvents.First().Id);
            Assert.Equal(expectedEvents.Last().Id, actualEvents.Last().Id);
        }

        [Fact]
        public async Task Get_ReturnsEmptyListWhenNoEvents()
        {
            await EventDatabaseSeeding.ClearEventTableAsync(_context);

            ActionResult<IEnumerable<Event>> result = await _eventController.Get();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            IEnumerable<Event> events = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);

            Assert.NotNull(events);
            Assert.Empty(events);

            await EventDatabaseSeeding.SeedWithDefaultEvents(_context);
        }

        // This test fails only when running many tests at once.
        // Needs further examination.

        //[Fact]
        //public async Task Get_ReturnsCorrectListOfEventsWhenManyEvents()
        //{
        //    int numberOfVenues = 100;

        //    await EventDatabaseSeeding.ClearEventTableAsync(_context);
        //    await EventDatabaseSeeding.CreateManyEventEntries(_context, numberOfVenues);

        //    IEnumerable<Event> expectedEvents = await _context.Events.ToListAsync();

        //    ActionResult<IEnumerable<Event>> result = await _eventController.Get();
        //    OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);

        //    IEnumerable<Event> actualEvents = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);

        //    Assert.NotNull(actualEvents);
        //    Assert.Equal(numberOfVenues, actualEvents.Count());
        //    Assert.Equal(expectedEvents.First().Id, actualEvents.First().Id);
        //    Assert.Equal(expectedEvents.Last().Id, actualEvents.Last().Id);
        //}

        [Fact]
        public async Task GetByField_ReturnsBadRequestOnArgumentException()
        {
            ActionResult<IEnumerable<Event>> result = await _eventController.GetByField(EventFields.Invalid, "Test");

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByField_ReturnsOkOnValidRequest()
        {
            ActionResult<IEnumerable<Event>> result = await _eventController.GetByField(EventFields.Id, DummyData.event1.Id.ToString());
            Assert.IsType<OkObjectResult>(result.Result);

            result = await _eventController.GetByField(EventFields.Name, DummyData.event1.Name);
            Assert.IsType<OkObjectResult>(result.Result);

            result = await _eventController.GetByField(EventFields.Date, DummyData.event1.Date.ToString());
            Assert.IsType<OkObjectResult>(result.Result);

            result = await _eventController.GetByField(EventFields.VenueId, DummyData.event1.VenueId.ToString());
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByField_ReturnsNotFoundOnEmptyRequestReturn()
        {
            ActionResult<IEnumerable<Event>> result = await _eventController.GetByField(EventFields.Id, Guid.NewGuid().ToString());
            Assert.IsType<NotFoundResult>(result.Result);

            result = await _eventController.GetByField(EventFields.Name, "");
            Assert.IsType<NotFoundResult>(result.Result);

            result = await _eventController.GetByField(EventFields.Date, new DateOnly().ToString());
            Assert.IsType<NotFoundResult>(result.Result);

            result = await _eventController.GetByField(EventFields.VenueId, Guid.NewGuid().ToString());
            Assert.IsType<NotFoundResult>(result.Result);
        }

    }
}
