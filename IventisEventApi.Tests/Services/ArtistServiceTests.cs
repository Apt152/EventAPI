using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using IventisEventApi.Tests.Database;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Services
{
    public class ArtistServiceTests
    {
        [Fact]
        public async Task GetArtistByIdAsync_ReturnsCorrectArtist()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            Artist testArtist = await dbContext.Artists.FirstAsync();
            Artist? resultArtist = await artistService.GetArtistByIdAsync(testArtist.Id);

            Assert.NotNull(resultArtist);
            Assert.Equal(testArtist.Id, resultArtist.Id);
            Assert.Equal(testArtist.Name, resultArtist.Name);
            Assert.Equal(testArtist.Genre, resultArtist.Genre);
        }

        [Fact]
        public async Task GetArtistByIdAsync_ReturnsNullForNoMatchingArtist()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            Guid randomGuid = Guid.NewGuid();

            Artist? resultArtist = await artistService.GetArtistByIdAsync(randomGuid);

            Assert.Null(resultArtist);
        }

        [Fact]
        public async Task GetArtistByNameAsync_ReturnsCorrectArtist()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            Artist testArtist = await dbContext.Artists.FirstAsync();
            IEnumerable<Artist> resultArtist = await artistService.GetArtistByNameAsync(testArtist.Name);

            Assert.NotNull(resultArtist);
            Assert.Equal(testArtist.Id, resultArtist.First().Id);
            Assert.Equal(testArtist.Name, resultArtist.First().Name);
            Assert.Equal(testArtist.Genre, resultArtist.First().Genre);
        }

        [Fact]
        public async Task GetArtistByNameAsync_ReturnsEmptyForNoMatchingArtist()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            IEnumerable<Artist> resultArtist = await artistService.GetArtistByNameAsync("");

            Assert.Empty(resultArtist);
        }

        [Fact]
        public async Task GetArtistByNameAsync_ReturnsMultipleCorrectArtists()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            IEnumerable<Artist> resultArtist = await artistService.GetArtistByNameAsync(DummyData.artist3.Name);

            Assert.NotNull(resultArtist);
            Assert.Contains(DummyData.artist3, resultArtist);
            Assert.Contains(DummyData.artist4, resultArtist);
            Assert.Equal(2, resultArtist.Count());
        }

        [Fact]
        public async Task GetArtistByGenreAsync_ReturnsCorrectArtist()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            Artist testArtist = await dbContext.Artists.FirstAsync();
            IEnumerable<Artist> resultArtist = await artistService.GetArtistByGenreAsync(testArtist.Genre);

            Assert.NotNull(resultArtist);
            Assert.Equal(testArtist.Id, resultArtist.First().Id);
            Assert.Equal(testArtist.Name, resultArtist.First().Name);
            Assert.Equal(testArtist.Genre, resultArtist.First().Genre);
        }

        [Fact]
        public async Task GetArtistByGenreAsync_ReturnsEmptyForNoMatchingArtist()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            IEnumerable<Artist> resultArtist = await artistService.GetArtistByGenreAsync("");

            Assert.Empty(resultArtist);
        }

        [Fact]
        public async Task GetArtistByGenreAsync_ReturnsMultipleCorrectArtists()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            IEnumerable<Artist> resultArtist = await artistService.GetArtistByGenreAsync(DummyData.artist1.Genre);

            Assert.NotNull(resultArtist);
            Assert.Contains(DummyData.artist1, resultArtist);
            Assert.Contains(DummyData.artist4, resultArtist);
            Assert.Equal(2, resultArtist.Count());
        }

        [Fact]
        public async Task AddArtistAsync_AddsArtistSuccessfully()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            Artist newArtist = DummyData.artist3;
            newArtist.Id = Guid.NewGuid();
            await artistService.AddArtistAsync(newArtist);
            Artist? resultArtist = await dbContext.Artists.FindAsync(newArtist.Id);

            Assert.NotNull(resultArtist);
            Assert.Equal(newArtist.Id, resultArtist.Id);
            Assert.Equal(newArtist.Name, resultArtist.Name);
            Assert.Equal(newArtist.Genre, resultArtist.Genre);
        }

        [Fact]
        public async Task AddArtistAsync_DoesNotAddNullArtist()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            Artist? incompleteArtist = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => artistService.AddArtistAsync(incompleteArtist));
        }

        [Fact]
        public async Task AddArtistAsync_DoesNotAddIncompleteArtist()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            Artist incompleteArtist = new() { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => artistService.AddArtistAsync(incompleteArtist));
            Artist? resultArtist = await dbContext.Artists.FindAsync(incompleteArtist.Id);

            Assert.Null(resultArtist);
        }

        [Fact]
        public async Task GetAllArtistsAsync_GetsCorrectAmount()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            int count = dbContext.Artists.Count();
            List<Artist> allArtists = await artistService.GetAllArtistsAsync();
            Assert.Equal(count, allArtists.Count);
        }

        [Fact]
        public async Task GetAllArtistsAsync_ReturnsEmptyListIfNone()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewEmptyDatabase();
            ArtistService artistService = new(dbContext);

            List<Artist> allArtists = await artistService.GetAllArtistsAsync();
            Assert.Empty(allArtists);
        }

        [Fact]
        public async Task GetAllArtistsAsync_GetsCorrectAmountWhenManyEntries()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewEmptyDatabase();
            ArtistService artistService = new(dbContext);

            int amountOfEntries = 100;
            await ArtistDatabaseSeeding.CreateManyArtistEntries(dbContext, amountOfEntries);

            List<Artist> allArtists = await artistService.GetAllArtistsAsync();
            Assert.Equal(amountOfEntries, allArtists.Count);
        }

        [Fact]
        public async Task GetArtistByEvent_ReturnsCorrectArtists()
        {
            using EventDbContext dbContext = await ArtistDatabaseSeeding.CreateNewDatabase();
            ArtistService artistService = new(dbContext);

            Event dummyEvent = DummyData.event2;
            List<Artist> resultArtists = await artistService.GetArtistsByEventAsync(dummyEvent.Id);

            Assert.Equal(2, resultArtists.Count);
            Assert.Contains(DummyData.artist1, resultArtists);
            Assert.Contains(DummyData.artist2, resultArtists);
        }


    }
}
