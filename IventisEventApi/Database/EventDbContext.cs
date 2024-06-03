using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace IventisEventApi.Database
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<EventArtist> EventsArtists { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Define properties of entities
            DefineVenueProperties(modelBuilder);

            // Define relationships between entities
            DefineEventArtistRelationship(modelBuilder);
            DefineEventVenueRelationship(modelBuilder);

            // Seed initial data
            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            Venue venue1 = new() { Id = Guid.NewGuid(), Name = "Venue 1", Capacity = 100, BoundingBox = new GeoBoundingBox(53.228317, -0.546745, 53.228295, -0.548888) };
            Venue venue2 = new() { Id = Guid.NewGuid(), Name = "Venue 2", Capacity = 50, BoundingBox = new GeoBoundingBox(52.912500, -1.183384, 52.909932, -1.186041) };
            modelBuilder.Entity<Venue>().HasData(
                venue1,
                venue2
                );

            Artist artist1 = new() { Id = Guid.NewGuid(), Name = "John Doe", Genre = "Pop" };
            Artist artist2 = new() { Id = Guid.NewGuid(), Name = "Jane Doe", Genre = "Country" };

            modelBuilder.Entity<Artist>().HasData(
                artist1,
                artist2
                );

            Event event1 = new() { Id = Guid.NewGuid(), Name = "Event 1", Date = new DateOnly(2024, 6, 4), VenueId = venue1.Id };
            Event event2 = new() { Id = Guid.NewGuid(), Name = "Event 2", Date = new DateOnly(2025, 6, 4), VenueId = venue2.Id };
            Event event3 = new() { Id = Guid.NewGuid(), Name = "Event 3", Date = new DateOnly(2024, 6, 7), VenueId = venue1.Id };

            modelBuilder.Entity<Event>().HasData(
                event1,
                event2,
                event3
                );

            modelBuilder.Entity<EventArtist>().HasData(
                new EventArtist { ArtistId = artist1.Id, EventId = event1.Id },
                new EventArtist { ArtistId = artist1.Id, EventId = event3.Id },
                new EventArtist { ArtistId = artist2.Id, EventId = event1.Id },
                new EventArtist { ArtistId = artist2.Id, EventId = event2.Id },
                new EventArtist { ArtistId = artist2.Id, EventId = event3.Id }
            );
        }


        private static void DefineVenueProperties(ModelBuilder modelBuilder)
        {
            ValueConverter<GeoBoundingBox, string> converter = new(
                v => GeoBoundingBox.ConvertToString(v),
                v => GeoBoundingBox.ConvertFromString(v));

            modelBuilder.Entity<Venue>()
                .Property(v => v.BoundingBox)
                .HasConversion(converter);
        }
        private static void DefineEventVenueRelationship(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(f => f.Events)
                .HasForeignKey(e => e.VenueId);
        }

        private static void DefineEventArtistRelationship(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventArtist>()
                .HasKey(e => new { e.EventId, e.ArtistId });

            modelBuilder.Entity<EventArtist>()
                .HasOne(e => e.Event)
                .WithMany(f => f.EventArtists)
                .HasForeignKey(e => e.EventId);

            modelBuilder.Entity<EventArtist>()
                .HasOne(e => e.Artist)
                .WithMany(f => f.EventArtists)
                .HasForeignKey(e => e.ArtistId);
        }





    }
}
