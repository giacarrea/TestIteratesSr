using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using EventManagerApi.Models;
using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;

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
        public async Task GetEvents_ReturnsOk()
        {
            var client = _factory.CreateClient();
            // Add authentication header if required

            var response = await client.GetAsync("/api/Event");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RegisterForEvent_ReturnsCreated()
        {
            var client = _factory.CreateClient();
            // Add authentication header if required

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
            createResponse.EnsureSuccessStatusCode();
            var createdEvent = await createResponse.Content.ReadFromJsonAsync<Event>();

            // Now, register for the event
            // You may need to authenticate as a user here
            var regResponse = await client.PostAsync($"/api/Event/{createdEvent.Id}/registrations", null);
            Assert.Equal(HttpStatusCode.Created, regResponse.StatusCode);
        }
    }
}
