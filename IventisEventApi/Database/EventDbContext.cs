using IventisEventApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            modelBuilder.Entity<Venue>().HasData(
                DummyData.venue1,
                DummyData.venue2
                );

            modelBuilder.Entity<Artist>().HasData(
                DummyData.artist1,
                DummyData.artist2
                );

            modelBuilder.Entity<Event>().HasData(
                DummyData.event1,
                DummyData.event2,
                DummyData.event3
                );

            modelBuilder.Entity<EventArtist>().HasData(
                new EventArtist { ArtistId = DummyData.artist1.Id, EventId = DummyData.event1.Id },
                new EventArtist { ArtistId = DummyData.artist1.Id, EventId = DummyData.event3.Id },
                new EventArtist { ArtistId = DummyData.artist2.Id, EventId = DummyData.event1.Id },
                new EventArtist { ArtistId = DummyData.artist2.Id, EventId = DummyData.event2.Id },
                new EventArtist { ArtistId = DummyData.artist2.Id, EventId = DummyData.event3.Id }
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
