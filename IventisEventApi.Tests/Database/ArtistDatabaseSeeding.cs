using IventisEventApi.Database;
using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Database
{
    internal class ArtistDatabaseSeeding
    {
        public static async Task SeedWithDefaultArtists(EventDbContext context)
        {
            if (context.Artists.Any())
            {
                await ClearArtistTableAsync(context);
            }
            context.Artists.AddRange(DummyData.artist1, DummyData.artist2);
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
    }
}
