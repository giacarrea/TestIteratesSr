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
}
