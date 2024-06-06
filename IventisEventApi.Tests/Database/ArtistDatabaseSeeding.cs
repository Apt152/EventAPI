using IventisEventApi.Database;
using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Database
{
    internal class ArtistDatabaseSeeding
    {
        public static async Task<EventDbContext> CreateNewDatabase()
        {
            DbContextOptions<EventDbContext> options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            EventDbContext context = new(options);
            await SeedWithDefaultArtists(context);
            return context;
        }
        
        public static async Task<EventDbContext> CreateNewEmptyDatabase()
        {
            DbContextOptions<EventDbContext> options = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            EventDbContext context = new(options);
            return context;
        }


        public static async Task SeedWithDefaultArtists(EventDbContext context)
        {
            if (context.Artists.Any())
            {
                await ClearArtistTableAsync(context);
            }
            context.Artists.AddRange(DummyData.artist1, DummyData.artist2, DummyData.artist3, DummyData.artist4);
            context.Venues.AddRange(DummyData.venue1,  DummyData.venue2);
            context.Events.AddRange(DummyData.event1, DummyData.event2, DummyData.event3);
            context.EventsArtists.AddRange(DummyData.eventArtist1,  DummyData.eventArtist2, DummyData.eventArtist3);
            await context.SaveChangesAsync();
        }

        public static async Task CreateManyArtistEntries(EventDbContext context, int amount)
        {
            IEnumerable<Artist> newArtists = [];
            for (int i = 0; i < amount; ++i)
            {
                newArtists = newArtists.Append(new Artist() { Id = Guid.NewGuid(), Name = "Artist" + i.ToString(), Genre = "Test" });
            }
            context.Artists.AddRange(newArtists);
            await context.SaveChangesAsync();
        }

        public static async Task ClearArtistTableAsync(EventDbContext context)
        {
            List<Artist> artistsInTable = await context.Artists.ToListAsync();
            context.Artists.RemoveRange(artistsInTable);
            await context.SaveChangesAsync();
        }

        public static async Task RevertToSeeded(EventDbContext context)
        {
            await ClearArtistTableAsync(context);
            await SeedWithDefaultArtists(context);
        }
    }
}
