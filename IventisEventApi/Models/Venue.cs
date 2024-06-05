using System.Text.Json.Serialization;

namespace IventisEventApi.Models
{
    public class Venue : IEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public GeoBoundingBox BoundingBox { get; set; }
        public int Capacity { get; set; }
        [JsonIgnore]
        public ICollection<Event> Events { get; set; } = [];

        public bool IsComplete()
        {
            if (Id == Guid.Empty) return false;
            if (string.IsNullOrEmpty(Name)) return false;
            if (Capacity == 0) return false;
            return true;
        }
    }
}
