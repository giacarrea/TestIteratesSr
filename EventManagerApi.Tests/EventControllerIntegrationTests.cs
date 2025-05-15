using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using EventManagerApi.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EventManagerApi.Tests
{
    public class EventControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public EventControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task LoginRoute_ReturnsJwtToken()
        {
            var client = _factory.CreateClient();

            // Attempt to get JWT token for a test user (ensure this user exists in your test DB)
            var loginRequest = new { Username = "admin", Password = "admin" };
            var response = await client.PostAsJsonAsync("/api/Auth/login", loginRequest);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var token = json.GetProperty("token").GetString();

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public async Task GetEvents_ReturnsOk()
        {
            var client = _factory.CreateClient();

            // Get JWT token for a test user (ensure this user exists in your test DB)
            var token = await GetJwtTokenAsync(client, "user1", "user1");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/Event");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateEventAsUser_Forbidden()
        {
            var client = _factory.CreateClient();

            // Get JWT token for a test user (ensure this user exists in your test DB)
            var token = await GetJwtTokenAsync(client, "user1", "user1");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // First, create an event as admin (mock or seed as needed)
            var newEvent = new Event
            {
                Title = "Test Event",
                Description = "Test Desc",
                Date = DateTime.UtcNow.AddDays(1),
                Location = "Test Location",
                Category = "Test",
                Capacity = 10
            };

            // You may need to authenticate as admin here
            var createResponse = await client.PostAsJsonAsync("/api/Event", newEvent);
            Assert.Equal(HttpStatusCode.Forbidden, createResponse.StatusCode);
        }


        private async Task<string> GetJwtTokenAsync(HttpClient client, string username, string password)
        {
            var loginRequest = new { Username = username, Password = password };
            var response = await client.PostAsJsonAsync("/api/Auth/login", loginRequest);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("token").GetString()!;
        }
    }
}
