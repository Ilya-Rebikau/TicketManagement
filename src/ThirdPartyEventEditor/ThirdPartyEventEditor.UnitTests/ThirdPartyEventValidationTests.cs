using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Services;

namespace ThirdPartyEventEditor.UnitTests
{
    [TestFixture]
    public class Tests
    {
        private IService<ThirdPartyEvent> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var eventRepositoryMock = new Mock<IRepository<ThirdPartyEvent>>();   
            eventRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEvents());
            var events = await eventRepositoryMock.Object.GetAllAsync();
            _service = new EventService(eventRepositoryMock.Object);
        }

        private static IQueryable<ThirdPartyEvent> GetTestEvents()
        {
            var events = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Id = 1,
                    LayoutId = 1,
                    StartDate = new DateTime(2030, 12, 21, 15, 0, 0),
                    EndDate = new DateTime(2030, 12, 21, 17, 0, 0),
                },
            };
            return events.AsQueryable();
        }

        [Test]
        public void CreateEvent_WhenTimeIsBusy_ShouldReturnArgumentException()
        {
            // Arrange
            ThirdPartyEvent eventModel = new()
            {
                Id = 2,
                LayoutId = 1,
                StartDate = new DateTime(2025, 12, 21, 15, 10, 0),
                EndDate = new DateTime(2035, 12, 21, 16, 40, 0),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenDatesInPast_ShouldReturnArgumentException()
        {
            // Arrange
            ThirdPartyEvent eventModel = new()
            {
                StartDate = new DateTime(2000, 1, 1),
                EndDate = new DateTime(2022, 2, 2),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenWrongDates_ShouldReturnArgumentException()
        {
            // Arrange
            ThirdPartyEvent eventModel = new()
            {
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2022, 2, 2),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenDatesInPast_ShouldReturnArgumentException()
        {
            // Arrange
            ThirdPartyEvent eventModel = new()
            {
                StartDate = new DateTime(2000, 1, 1),
                EndDate = new DateTime(2022, 2, 2),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenWrongDates_ShouldReturnArgumentException()
        {
            // Arrange
            ThirdPartyEvent eventModel = new()
            {
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2022, 2, 2),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}