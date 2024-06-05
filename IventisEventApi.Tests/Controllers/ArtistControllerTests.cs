using IventisEventApi.Controllers;
using IventisEventApi.Database;
using IventisEventApi.ModelFields;
using IventisEventApi.Models;
using IventisEventApi.Tests.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Controllers
{
    public class ArtistControllerTests : IAsyncLifetime
    {
        private readonly DbContextOptions<EventDbContext> _options;
        private readonly EventDbContext _context;
        private readonly ArtistController _artistController;

        public ArtistControllerTests()
        {
            _options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new EventDbContext(_options);
            _artistController = new ArtistController(_context);
        }

        public async Task InitializeAsync()
        {
            await ArtistDatabaseSeeding.SeedWithDefaultArtists(_context);
        }

        public Task DisposeAsync()
        {
            _context.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Get_ReturnsCorrectListOfArtists()
        {
            IEnumerable<Artist> expectedArtists = await _context.Artists.ToListAsync();
            ActionResult<IEnumerable<Artist>> result = await _artistController.Get();
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);

            IEnumerable<Artist> actualArtists = Assert.IsAssignableFrom<IEnumerable<Artist>>(okResult.Value);

            Assert.NotNull(actualArtists);
            Assert.Equal(expectedArtists.Count(), actualArtists.Count());
            Assert.Equal(expectedArtists.First().Id, actualArtists.First().Id);
            Assert.Equal(expectedArtists.Last().Id, actualArtists.Last().Id);
        }

        [Fact]
        public async Task Get_ReturnsEmptyListWhenNoArtists()
        {
            await ArtistDatabaseSeeding.ClearArtistTableAsync(_context);

            ActionResult<IEnumerable<Artist>> result = await _artistController.Get();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            IEnumerable<Artist> artists = Assert.IsAssignableFrom<IEnumerable<Artist>>(okResult.Value);

            Assert.NotNull(artists);
            Assert.Empty(artists);

            await ArtistDatabaseSeeding.SeedWithDefaultArtists(_context);
        }

        [Fact]
        public async Task Get_ReturnsCorrectListOfArtistsWhenManyArtists()
        {
            int numberOfVenues = 100;

            await ArtistDatabaseSeeding.ClearArtistTableAsync(_context);
            await ArtistDatabaseSeeding.CreateManyArtistEntries(_context, numberOfVenues);

            IEnumerable<Artist> expectedArtists = await _context.Artists.ToListAsync();

            ActionResult<IEnumerable<Artist>> result = await _artistController.Get();
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);

            IEnumerable<Artist> actualArtists = Assert.IsAssignableFrom<IEnumerable<Artist>>(okResult.Value);

            Assert.NotNull(actualArtists);
            Assert.Equal(numberOfVenues, actualArtists.Count());
            Assert.Equal(expectedArtists.First().Id, actualArtists.First().Id);
            Assert.Equal(expectedArtists.Last().Id, actualArtists.Last().Id);
        }

        [Fact]
        public async Task GetByField_ReturnsBadRequestOnArgumentException()
        {
            ActionResult<IEnumerable<Artist>> result = await _artistController.GetByField(ArtistFields.Invalid, "Test");

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByField_ReturnsOkOnValidRequest()
        {
            ActionResult<IEnumerable<Artist>> result = await _artistController.GetByField(ArtistFields.Id, DummyData.artist1.Id.ToString());
            Assert.IsType<OkObjectResult>(result.Result);

            result = await _artistController.GetByField(ArtistFields.Name, DummyData.artist1.Name);
            Assert.IsType<OkObjectResult>(result.Result);

            result = await _artistController.GetByField(ArtistFields.Genre, DummyData.artist1.Genre);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByField_ReturnsNotFoundOnEmptyRequestReturn()
        {
            ActionResult<IEnumerable<Artist>> result = await _artistController.GetByField(ArtistFields.Id, Guid.NewGuid().ToString());
            Assert.IsType<NotFoundResult>(result.Result);

            result = await _artistController.GetByField(ArtistFields.Name, "");
            Assert.IsType<NotFoundResult>(result.Result);

            result = await _artistController.GetByField(ArtistFields.Genre, "");
            Assert.IsType<NotFoundResult>(result.Result);
        }

    }
}
