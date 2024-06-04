namespace IventisEventApi.Models
{
    public class Event : IEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateOnly Date { get; set; }
        public Guid VenueId { get; set; }
        public Venue Venue { get; set; } = null!;


        public ICollection<EventArtist> EventArtists { get; set; } = [];

        public bool IsComplete()
        {
            if (Id == Guid.Empty) return false;
            if (string.IsNullOrEmpty(Name)) return false;
            if (VenueId == Guid.Empty) return false;
            return true;
        }

    }
}
