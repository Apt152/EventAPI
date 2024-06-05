using System.Text.Json.Serialization;

namespace IventisEventApi.Models
{
    public class EventArtist
    {
        public Guid EventId { get; set; }
        [JsonIgnore]
        public Event Event { get; set; } = null!;
        public Guid ArtistId { get; set; }
        [JsonIgnore]
        public Artist Artist { get; set; } = null!;
    }
}
