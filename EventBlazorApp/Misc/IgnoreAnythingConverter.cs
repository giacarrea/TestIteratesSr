using System.Text.Json.Serialization;
using System.Text.Json;
using EventBlazorApp.Models;

namespace EventBlazorApp.Misc
{
    public class IgnoreAnythingConverter : JsonConverter<EventRefDto?>
    {
        public override EventRefDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Skip the value, return null
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using (JsonDocument doc = JsonDocument.ParseValue(ref reader)) { }
                return null;
            }
            if (reader.TokenType == JsonTokenType.String)
            {
                reader.GetString();
                return null;
            }
            reader.Skip();
            return null;
        }

        public override void Write(Utf8JsonWriter writer, EventRefDto? value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
