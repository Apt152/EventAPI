using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
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
            await SeedDatabaseAsync();
        }

        public Task DisposeAsync()
        {
            _context.Dispose();
            return Task.CompletedTask;
        }

        private async Task SeedDatabaseAsync()
        {
            if (!_context.Artists.Any())
            {
                _context.Artists.AddRange(DummyData.artist1, DummyData.artist2);
                await _context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetArtistByIdAsync_ReturnsCorrectArtist()
        {
            Artist testArtist = await _context.Artists.FirstAsync();
            Artist? resultArtist = await _artistService.GetArtistById(testArtist.Id);

            Assert.NotNull(resultArtist);
            Assert.Equal(testArtist.Id, resultArtist.Id);
            Assert.Equal(testArtist.Name, resultArtist.Name);
            Assert.Equal(testArtist.Genre, resultArtist.Genre);
        }

        [Fact]
        public async Task GetArtistByIdAsync_ReturnsNullForMissingArtist()
        {
            Guid randomGuid = Guid.NewGuid();

            Artist? resultArtist = await _artistService.GetArtistById(randomGuid);

            Assert.Null(resultArtist);
        }

        [Fact]
        public async Task AddArtistAsync_AddsArtistSuccessfully()
        {
            Artist newArtist = DummyData.artist3;
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
        
        [Fact]
        public async Task GetAllArtistsAsync_ReturnsEmptyListIfNone()
        {
            await ClearArtistTableAsync();            

            List<Artist> allArtists = await _artistService.GetAllArtistsAsync();
            Assert.Empty(allArtists);
        }

        [Fact]
        public async Task GetAllArtistsAsync_GetsCorrectAmountWhenManyEntries()
        {
            await ClearArtistTableAsync();

            int amountOfEntries = 1000;
            await CreateManyArtistEntries(amountOfEntries);

            List<Artist> allArtists = await _artistService.GetAllArtistsAsync();
            Assert.Equal(amountOfEntries, allArtists.Count);
        }

        private async Task CreateManyArtistEntries(int amount)
        {
            IEnumerable<Artist> newArtists = [];
            for (int i = 0; i < amount; ++i)
            {
                newArtists = newArtists.Append(new Artist() { Id = Guid.NewGuid(), Name = "Artist" + i.ToString(), Genre = "Test" });
            }
            _context.Artists.AddRange(newArtists);
            await _context.SaveChangesAsync();
        }

        private async Task ClearArtistTableAsync()
        {
            List<Artist> artistsInTable = await _context.Artists.ToListAsync();
            _context.Artists.RemoveRange(artistsInTable);
            await _context.SaveChangesAsync();
        }
    }
}
