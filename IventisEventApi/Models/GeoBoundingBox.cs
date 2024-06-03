namespace IventisEventApi.Models
{
    public struct GeoBoundingBox
    {
        public GeoLocation Vertex1;
        public GeoLocation Vertex2;

        public GeoBoundingBox(GeoLocation vertex1, GeoLocation vertex2)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
        }

        public GeoBoundingBox(double vertex1Latitude, double vertex1Longitude,  double vertex2Latitude, double vertex2Longitude)
        {
            this.Vertex1 = new GeoLocation(vertex1Latitude, vertex1Longitude);
            this.Vertex2 = new GeoLocation(vertex2Latitude, vertex2Longitude);
        }
    }
}
