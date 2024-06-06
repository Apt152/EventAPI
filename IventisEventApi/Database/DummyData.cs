using IventisEventApi.Models;
using Microsoft.Extensions.Logging;

namespace IventisEventApi.Database
{
    public readonly struct DummyData
    {
        public static readonly Venue venue1;
        public static readonly Venue venue2;
        public static readonly Venue venue3;

        public static readonly Artist artist1;
        public static readonly Artist artist2;
        public static readonly Artist artist3;
        public static readonly Artist artist4;

        public static readonly Event event1;
        public static readonly Event event2;
        public static readonly Event event3;

        public static readonly EventArtist eventArtist1;
        public static readonly EventArtist eventArtist2;
        public static readonly EventArtist eventArtist3;


        public static readonly GeoBoundingBox boundingBox1 = new(1.0, 2.0, 3.0, 4.0);
        public static readonly GeoBoundingBox boundingBox2 = new(5.0, 6.0, 7.0, 8.0);

        static DummyData()
        {
            venue1 = new() { Id = Guid.NewGuid(), Name = "Venue 1", Capacity = 100, BoundingBox = new GeoBoundingBox(53.228317, -0.546745, 53.228295, -0.548888) };
            venue2 = new() { Id = Guid.NewGuid(), Name = "Venue 2", Capacity = 50, BoundingBox = new GeoBoundingBox(52.912500, -1.183384, 52.909932, -1.186041) };
            venue3 = new() { Id = Guid.NewGuid(), Name = "Venue 3", Capacity = 50, BoundingBox = new GeoBoundingBox(52.953662, -1.150368, 52.953197, -1.149475) };

            artist1 = new() { Id = Guid.NewGuid(), Name = "John Doe", Genre = "Pop" };
            artist2 = new() { Id = Guid.NewGuid(), Name = "Jane Doe", Genre = "Country" };
            artist3 = new() { Id = Guid.NewGuid(), Name = "John Smith", Genre = "Classical" };
            artist4 = new() { Id = Guid.NewGuid(), Name = "John Smith", Genre = "Pop" };

            event1 = new() { Id = Guid.NewGuid(), Name = "Event 1", Date = new DateOnly(2024, 6, 4), VenueId = venue1.Id };
            event2 = new() { Id = Guid.NewGuid(), Name = "Event 2", Date = new DateOnly(2025, 6, 4), VenueId = venue2.Id };
            event3 = new() { Id = Guid.NewGuid(), Name = "Event 3", Date = new DateOnly(2024, 6, 7), VenueId = venue1.Id };

            eventArtist1 = new() { ArtistId = artist1.Id, EventId = event1.Id };
            eventArtist2 = new() { ArtistId = artist1.Id, EventId = event2.Id };
            eventArtist3 = new() { ArtistId = artist2.Id, EventId = event2.Id };
        }

}
}
