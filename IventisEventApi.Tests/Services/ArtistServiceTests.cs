using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using IventisEventApi.Tests.Database;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Services
{
    public class ArtistServiceTests : IAsyncLifetime
    {
        private readonly DbContextOptions<EventDbContext> _options;
        private readonly EventDbContext _context;
        private readonly ArtistService _artistService;

        public ArtistServiceTests()
        {
            _options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new EventDbContext(_options);
            _artistService = new ArtistService(_context);
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
        public async Task GetArtistByIdAsync_ReturnsCorrectArtist()
        {
            Artist testArtist = await _context.Artists.FirstAsync();
            Artist? resultArtist = await _artistService.GetArtistByIdAsync(testArtist.Id);

            Assert.NotNull(resultArtist);
            Assert.Equal(testArtist.Id, resultArtist.Id);
            Assert.Equal(testArtist.Name, resultArtist.Name);
            Assert.Equal(testArtist.Genre, resultArtist.Genre);
        }

        [Fact]
        public async Task GetArtistByIdAsync_ReturnsNullForNoMatchingArtist()
        {
            Guid randomGuid = Guid.NewGuid();

            Artist? resultArtist = await _artistService.GetArtistByIdAsync(randomGuid);

            Assert.Null(resultArtist);
        }

        [Fact]
        public async Task GetArtistByNameAsync_ReturnsCorrectArtist()
        {
            Artist testArtist = await _context.Artists.FirstAsync();
            IEnumerable<Artist> resultArtist = await _artistService.GetArtistByNameAsync(testArtist.Name);

            Assert.NotNull(resultArtist);
            Assert.Equal(testArtist.Id, resultArtist.First().Id);
            Assert.Equal(testArtist.Name, resultArtist.First().Name);
            Assert.Equal(testArtist.Genre, resultArtist.First().Genre);
        }

        [Fact]
        public async Task GetArtistByNameAsync_ReturnsEmptyForNoMatchingArtist()
        {
            IEnumerable<Artist> resultArtist = await _artistService.GetArtistByNameAsync("");

            Assert.Empty(resultArtist);
        }

        [Fact]
        public async Task GetArtistByNameAsync_ReturnsMultipleCorrectArtists()
        {
            IEnumerable<Artist> resultArtist = await _artistService.GetArtistByNameAsync(DummyData.artist3.Name);

            Assert.NotNull(resultArtist);
            Assert.Contains(DummyData.artist3, resultArtist);
            Assert.Contains(DummyData.artist4, resultArtist);
            Assert.Equal(2, resultArtist.Count());
        }

        [Fact]
        public async Task GetArtistByGenreAsync_ReturnsCorrectArtist()
        {
            Artist testArtist = await _context.Artists.FirstAsync();
            IEnumerable<Artist> resultArtist = await _artistService.GetArtistByGenreAsync(testArtist.Genre);

            Assert.NotNull(resultArtist);
            Assert.Equal(testArtist.Id, resultArtist.First().Id);
            Assert.Equal(testArtist.Name, resultArtist.First().Name);
            Assert.Equal(testArtist.Genre, resultArtist.First().Genre);
        }

        [Fact]
        public async Task GetArtistByGenreAsync_ReturnsEmptyForNoMatchingArtist()
        {
            IEnumerable<Artist> resultArtist = await _artistService.GetArtistByGenreAsync("");

            Assert.Empty(resultArtist);
        }

        [Fact]
        public async Task GetArtistByGenreAsync_ReturnsMultipleCorrectArtists()
        {
            IEnumerable<Artist> resultArtist = await _artistService.GetArtistByGenreAsync(DummyData.artist1.Genre);

            Assert.NotNull(resultArtist);
            Assert.Contains(DummyData.artist1, resultArtist);
            Assert.Contains(DummyData.artist4, resultArtist);
            Assert.Equal(2, resultArtist.Count());
        }

        [Fact]
        public async Task AddArtistAsync_AddsArtistSuccessfully()
        {
            Artist newArtist = DummyData.artist3;
            newArtist.Id = Guid.NewGuid();
            await _artistService.AddArtistAsync(newArtist);
            Artist? resultArtist = await _context.Artists.FindAsync(newArtist.Id);

            Assert.NotNull(resultArtist);
            Assert.Equal(newArtist.Id, resultArtist.Id);
            Assert.Equal(newArtist.Name, resultArtist.Name);
            Assert.Equal(newArtist.Genre, resultArtist.Genre);
        }

        [Fact]
        public async Task AddArtistAsync_DoesNotAddNullArtist()
        {
            Artist? incompleteArtist = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _artistService.AddArtistAsync(incompleteArtist));
        }

        [Fact]
        public async Task AddArtistAsync_DoesNotAddIncompleteArtist()
        {
            Artist incompleteArtist = new() { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => _artistService.AddArtistAsync(incompleteArtist));
            Artist? resultArtist = await _context.Artists.FindAsync(incompleteArtist.Id);

            Assert.Null(resultArtist);
        }

        [Fact]
        public async Task GetAllArtistsAsync_GetsCorrectAmount()
        {
            int count = _context.Artists.Count();
            List<Artist> allArtists = await _artistService.GetAllArtistsAsync();
            Assert.Equal(count, allArtists.Count);
        }
        
        //[Fact]
        //public async Task GetAllArtistsAsync_ReturnsEmptyListIfNone()
        //{
        //    await ArtistDatabaseSeeding.ClearArtistTableAsync(_context);

        //    List<Artist> allArtists = await _artistService.GetAllArtistsAsync();
        //    Assert.Empty(allArtists);

        //    await ArtistDatabaseSeeding.SeedWithDefaultArtists(_context);
        //}

        //[Fact]
        //public async Task GetAllArtistsAsync_GetsCorrectAmountWhenManyEntries()
        //{
        //    await ArtistDatabaseSeeding.ClearArtistTableAsync(_context);

        //    int amountOfEntries = 100;
        //    await ArtistDatabaseSeeding.CreateManyArtistEntries(_context, amountOfEntries);

        //    List<Artist> allArtists = await _artistService.GetAllArtistsAsync();
        //    Assert.Equal(amountOfEntries, allArtists.Count);

        //    await ArtistDatabaseSeeding.RevertToSeeded(_context);
        //}

        [Fact]
        public async Task GetArtistByEvent_ReturnsCorrectArtists()
        {
            Event dummyEvent = DummyData.event2;
            List<Artist> resultArtists = await _artistService.GetArtistsByEventAsync(dummyEvent.Id);

            Assert.Equal(2, resultArtists.Count);
            Assert.Contains(DummyData.artist1, resultArtists);
            Assert.Contains(DummyData.artist2, resultArtists);
        }


    }
}
