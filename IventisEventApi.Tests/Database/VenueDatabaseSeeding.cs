using IventisEventApi.Database;
using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Database
{
    internal class VenueDatabaseSeeding
    {
        public static async Task SeedWithDefaultVenues(EventDbContext context)
        {
            if (context.Venues.Any())
            {
                await ClearVenueTableAsync(context);
            }
            context.Venues.AddRange(DummyData.venue1, DummyData.venue2);
            await context.SaveChangesAsync();
        }

        public static async Task CreateManyVenueEntries(EventDbContext context, int amount)
        {
            IEnumerable<Venue> newVenues = [];
            for (int i = 0; i < amount; ++i)
            {
                newVenues = newVenues.Append(new Venue() { Id = Guid.NewGuid(), Name = "Venue" + i.ToString(), BoundingBox = DummyData.boundingBox1 });
            }
            context.Venues.AddRange(newVenues);
            await context.SaveChangesAsync();
        }

        public static async Task ClearVenueTableAsync(EventDbContext context)
        {
            List<Venue> venuesInTable = await context.Venues.ToListAsync();
            context.Venues.RemoveRange(venuesInTable);
            await context.SaveChangesAsync();
        }

        public static async Task RevertToSeeded(EventDbContext context)
        {
            await ClearVenueTableAsync(context);
            await SeedWithDefaultVenues(context);
        }
    }
}
