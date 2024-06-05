using IventisEventApi.Controllers;
using IventisEventApi.Database;
using IventisEventApi.Models;
using IventisEventApi.Services;
using IventisEventApi.Tests.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            IEnumerable<Artist> actualArtists = await _artistController.Get();

            Assert.NotNull(actualArtists);
            Assert.Equal(expectedArtists.Count(), actualArtists.Count());
            Assert.Equal(expectedArtists.First().Id, actualArtists.First().Id);
            Assert.Equal(expectedArtists.Last().Id, actualArtists.Last().Id);
        }

        [Fact]
        public async Task Get_ReturnsEmptyListWhenNoArtists()
        {
            await ArtistDatabaseSeeding.ClearArtistTableAsync(_context);

            IEnumerable<Artist> artists = await _artistController.Get();
            Assert.NotNull(artists);
            Assert.Empty(artists);

            await ArtistDatabaseSeeding.SeedWithDefaultArtists(_context);
        }

        [Fact]
        public async Task Get_ReturnsCorrectListOfArtistsWhenManyArtists()
        {
            await ArtistDatabaseSeeding.ClearArtistTableAsync(_context);
            await ArtistDatabaseSeeding.CreateManyArtistEntries(_context, 500);

            IEnumerable<Artist> expectedArtists = await _context.Artists.ToListAsync();
            IEnumerable<Artist> actualArtists = await _artistController.Get();

            Assert.NotNull(actualArtists);
            Assert.Equal(500, actualArtists.Count());
            Assert.Equal(expectedArtists.First().Id, actualArtists.First().Id);
            Assert.Equal(expectedArtists.Last().Id, actualArtists.Last().Id);
        }

    }
}
