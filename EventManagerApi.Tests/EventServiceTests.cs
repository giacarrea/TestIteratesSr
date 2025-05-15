using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using EventManagerApi.Services;
using EventManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagerApi.Interface;

namespace EventManagerApi.Tests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventDbContext> _dbContextMock = new();
        private readonly Mock<ILogger<EventService>> _loggerMock = new();

        [Fact]
        public async Task RegisterForEventAsync_ShouldThrow_WhenEventNotFound()
        {
            _dbContextMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Event?)null);
            var service = new EventService(_dbContextMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.RegisterForEventAsync(Guid.NewGuid(), "user1"));
        }

        [Fact]
        public async Task RegisterForEventAsync_ShouldThrow_WhenEventIsFull()
        {
            var ev = new Event { Id = Guid.NewGuid(), Capacity = 1, Registrations = new List<Registration> { new Registration() } };
            _dbContextMock.Setup(x => x.GetByIdAsync(ev.Id)).ReturnsAsync(ev);
            var service = new EventService(_dbContextMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.RegisterForEventAsync(ev.Id, "user2"));
        }

        [Fact]
        public async Task RegisterForEventAsync_ShouldThrow_WhenUserAlreadyRegistered()
        {
            var ev = new Event
            {
                Id = Guid.NewGuid(),
                Capacity = 2,
                Registrations = new List<Registration> { new Registration { UserId = "user1" } }
            };
            _dbContextMock.Setup(x => x.GetByIdAsync(ev.Id)).ReturnsAsync(ev);
            var service = new EventService(_dbContextMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.RegisterForEventAsync(ev.Id, "user1"));
        }

        [Fact]
        public async Task RegisterForEventAsync_ShouldSucceed_WhenValid()
        {
            var ev = new Event
            {
                Id = Guid.NewGuid(),
                Capacity = 2,
                Registrations = new List<Registration>()
            };
            _dbContextMock.Setup(x => x.GetByIdAsync(ev.Id)).ReturnsAsync(ev);
            _dbContextMock.Setup(x => x.AddRegistrationAsync(ev, It.IsAny<Registration>())).Returns(Task.CompletedTask);
            var service = new EventService(_dbContextMock.Object, _loggerMock.Object);

            var result = await service.RegisterForEventAsync(ev.Id, "user2");

            Assert.Equal("user2", result.UserId);
            _dbContextMock.Verify(x => x.AddRegistrationAsync(ev, It.IsAny<Registration>()), Times.Once);
        }
    }
}
