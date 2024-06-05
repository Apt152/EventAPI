using System.Text.Json;
using System.Text.Json.Serialization;

namespace IventisEventApi.Models
{
    public class GeoBoundingBoxConverter : JsonConverter<GeoBoundingBox>
    {
        public override GeoBoundingBox Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, GeoBoundingBox value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(GeoBoundingBox.ConvertToString(value));
        }
    }
}
