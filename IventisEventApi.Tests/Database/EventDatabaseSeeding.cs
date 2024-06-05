using IventisEventApi.Database;
using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;


namespace IventisEventApi.Tests.Database
{
    internal class EventDatabaseSeeding
    {
        public static async Task SeedWithDefaultEvents(EventDbContext context)
        {
            if (context.Events.Any())
            {
                await ClearEventTableAsync(context);
            }
            context.Events.AddRange(DummyData.event1, DummyData.event2);
            await context.SaveChangesAsync();
        }

        public static async Task CreateManyEventEntries(EventDbContext context, int amount)
        {
            IEnumerable<Event> newEvents = [];
            for (int i = 0; i < amount; ++i)
            {
                newEvents = newEvents.Append(new Event() { Id = Guid.NewGuid(), Name = "Event" + i.ToString(), VenueId = DummyData.venue1.Id, Date = new() });
            }
            context.Events.AddRange(newEvents);
            await context.SaveChangesAsync();
        }

        public static async Task ClearEventTableAsync(EventDbContext context)
        {
            List<Event> eventsInTable = await context.Events.ToListAsync();
            context.Events.RemoveRange(eventsInTable);
            await context.SaveChangesAsync();
        }

        public static async Task RevertToSeeded(EventDbContext context)
        {
            await ClearEventTableAsync(context);
            await SeedWithDefaultEvents(context);
        }
    }
}
