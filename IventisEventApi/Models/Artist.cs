namespace IventisEventApi.Models
{
    public class Artist
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public ICollection<EventArtist> EventArtists { get; set; } = [];

    }
}
