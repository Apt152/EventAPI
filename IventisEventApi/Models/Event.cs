namespace IventisEventApi.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public Guid venueId { get; set; }

    }
}
