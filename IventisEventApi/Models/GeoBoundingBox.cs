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

        public static string ConvertToString(GeoBoundingBox boundingBox)
        {
            List<double> values = [
                boundingBox.vertex1.latitude,
                boundingBox.vertex1.longitude,
                boundingBox.vertex2.latitude,
                boundingBox.vertex2.longitude 
                ];
            return string.Join("#", values);
        }

        public static GeoBoundingBox ConvertFromString(string values)
        {
            string[] parts = values.Split("#");
            if (parts.Length != 4)
            {
                throw new InvalidOperationException("Invalid bounding box format. Format as \"lat1#long1#lat2#long2\".");
            }

            GeoLocation v1 = new GeoLocation(double.Parse(parts[0]), double.Parse(parts[1]));
            GeoLocation v2 = new GeoLocation(double.Parse(parts[2]), double.Parse(parts[3]));

            return new GeoBoundingBox(v1, v2);
        }
    }
}
