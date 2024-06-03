namespace IventisEventApi.Models
{
    public struct GeoLocation(double latitude, double longitude)
    {
        public double latitude = latitude;
        public double longitude = longitude;
    }
}
