namespace IventisEventApi.Models
{
    public class EventArtist
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;
        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
    }
}
