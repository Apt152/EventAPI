using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Services
{
    public class ArtistServiceTests
    {
        private readonly DbContextOptions<EventDbContext> _options;
        private readonly EventDbContext _context;
        private readonly ArtistService _artistService;

        public ArtistServiceTests()
        {
            _options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new EventDbContext(_options);
            _artistService = new ArtistService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (!_context.Artists.Any())
            {
                _context.Artists.Add(DummyData.artist1);
                _context.Artists.Add(DummyData.artist2);
                _context.SaveChanges();
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
    }
}
