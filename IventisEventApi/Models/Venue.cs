//using System.Device.Location;

namespace IventisEventApi.Models
{
    public class Venue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GeoBoundingBox BoundingBox { get; set; }
        public int Capacity { get; set; }
        
    }
}
