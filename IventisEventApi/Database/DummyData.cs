using IventisEventApi.Models;

namespace IventisEventApi.Database
{
    public class DummyData
    {
        public static readonly Venue venue1 = new() { Id = Guid.NewGuid(), Name = "Venue 1", Capacity = 100, BoundingBox = new GeoBoundingBox(53.228317, -0.546745, 53.228295, -0.548888) };
        public static readonly Venue venue2 = new() { Id = Guid.NewGuid(), Name = "Venue 2", Capacity = 50, BoundingBox = new GeoBoundingBox(52.912500, -1.183384, 52.909932, -1.186041) };
        public static readonly Venue venue3 = new() { Id = Guid.NewGuid(), Name = "Venue 3", Capacity = 50, BoundingBox = new GeoBoundingBox(52.953662, -1.150368, 52.953197, -1.149475) };

        public static readonly Artist artist1 = new() { Id = Guid.NewGuid(), Name = "John Doe", Genre = "Pop" };
        public static readonly Artist artist2 = new() { Id = Guid.NewGuid(), Name = "Jane Doe", Genre = "Country" };
        public static readonly Artist artist3 = new() { Id = Guid.NewGuid(), Name = "John Smith", Genre = "Classical" };

        public static readonly Event event1 = new() { Id = Guid.NewGuid(), Name = "Event 1", Date = new DateOnly(2024, 6, 4), VenueId = venue1.Id };
        public static readonly Event event2 = new() { Id = Guid.NewGuid(), Name = "Event 2", Date = new DateOnly(2025, 6, 4), VenueId = venue2.Id };
        public static readonly Event event3 = new() { Id = Guid.NewGuid(), Name = "Event 3", Date = new DateOnly(2024, 6, 7), VenueId = venue1.Id };
    }
}
