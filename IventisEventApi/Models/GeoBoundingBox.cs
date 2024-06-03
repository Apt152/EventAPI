namespace IventisEventApi.Models
{
    public struct GeoBoundingBox
    {
        public GeoLocation vertex1;
        public GeoLocation vertex2;

        public GeoBoundingBox(GeoLocation vertex1, GeoLocation vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }

        public GeoBoundingBox(double vertex1Latitude, double vertex1Longitude,  double vertex2Latitude, double vertex2Longitude)
        {
            this.vertex1 = new GeoLocation(vertex1Latitude, vertex1Longitude);
            this.vertex2 = new GeoLocation(vertex2Latitude, vertex2Longitude);
        }
    }
}
