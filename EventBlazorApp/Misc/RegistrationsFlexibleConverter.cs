using EventBlazorApp.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RegistrationsFlexibleConverter : JsonConverter<List<RegistrationDto>>
{
    public override List<RegistrationDto>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // If the token is StartObject, try to read $values
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            if (doc.RootElement.TryGetProperty("$values", out var values))
            {
                return JsonSerializer.Deserialize<List<RegistrationDto>>(values.GetRawText(), options);
            }
            return new List<RegistrationDto>();
        }
        // If the token is StartArray, just deserialize as usual
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<List<RegistrationDto>>(ref reader, options);
        }
        // Otherwise, return empty
        return new List<RegistrationDto>();
    }

    public override void Write(Utf8JsonWriter writer, List<RegistrationDto> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
