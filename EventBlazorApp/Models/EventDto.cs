using System.Text.Json.Serialization;

namespace EventBlazorApp.Models
{
    public class EventListDto
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("$values")]
        public List<EventDto> Events { get; set; } = new();
    }

    public class EventDto
    {
        [JsonPropertyName("$id")]
        public string IdRef { get; set; } = "";

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; } = "";

        [JsonPropertyName("category")]
        public string Category { get; set; } = "";

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("registrations")]
        public RegistrationList RegistrationList { get; set; } = new();

        // Not from API, used for UI logic
        [JsonIgnore]
        public bool IsRegistered { get; set; }
    }

    public class RegistrationList
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("$values")]
        public List<RegistrationDto> Registrations { get; set; } = new();
    }

    public class RegistrationDto
    {
        [JsonPropertyName("$id")]
        public string IdRef { get; set; } = "";

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("eventId")]
        public Guid EventId { get; set; }

        [JsonPropertyName("event")]
        public EventRefDto? Event { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; } = "";

        [JsonPropertyName("registeredAt")]
        public DateTime RegisteredAt { get; set; }
    }

    public class EventRefDto
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; } = "";
    }
}
