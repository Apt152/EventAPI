namespace IventisEventApi.Models
{
    public class Artist
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Genre { get; set; }
        public ICollection<EventArtist> EventArtists { get; set; } = [];


        public bool IsComplete ()
        {
            if (Id == Guid.Empty) return false;
            if (Name == null) return false;
            if (Genre == null) return false;
            return true;
        }

    }
}
