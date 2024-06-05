using System.Text.Json.Serialization;

namespace IventisEventApi.Models
{
    public class Artist : IEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Genre { get; set; }
        [JsonIgnore]
        public ICollection<EventArtist> EventArtists { get; set; } = [];


        public bool IsComplete ()
        {
            if (Id == Guid.Empty) return false;
            if (string.IsNullOrEmpty(Name)) return false;
            if (string.IsNullOrEmpty(Genre)) return false;
            return true;
        }

    }
}
