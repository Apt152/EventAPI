namespace IventisEventApi.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public Guid VenueId { get; set; }
        public Venue Venue { get; set; } = null!;


        public ICollection<EventArtist> EventArtists { get; set; } = [];



    }
}
