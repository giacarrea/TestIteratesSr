using System.Text.Json.Serialization;

namespace EventBlazorApp.Models
{
    public class LoginModel
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = "";

        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
    }

    public class LoginResult
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("token")]
        public string Token { get; set; } = "";

        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }

        [JsonPropertyName("roleList")]
        public RoleList RoleList { get; set; } = new();
    }

    public class RoleList
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("$values")]
        public List<string> Roles { get; set; } = new();
    }
}
