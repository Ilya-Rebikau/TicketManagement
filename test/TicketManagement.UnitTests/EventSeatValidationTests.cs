using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;
using TicketManagement.EventManagerAPI.Services;

namespace TicketManagement.UnitTests
{
    [TestFixture]
    internal class EventSeatValidationTests
    {
        private IService<EventSeatDto> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            var eventSeatConverterMock = new Mock<IConverter<EventSeat, EventSeatDto>>();
            eventSeatRepositoryMock.Setup(rep => rep.GetAllAsync());
            var eventSeats = await eventSeatRepositoryMock.Object.GetAllAsync();
            eventSeatConverterMock.Setup(rep => rep.ConvertModelsRangeToDtos(eventSeats)).ReturnsAsync(GetTestEventSeatDtos());
            _service = new EventSeatService(eventSeatRepositoryMock.Object, eventSeatConverterMock.Object);
        }

        private static IEnumerable<EventSeatDto> GetTestEventSeatDtos()
        {
            IEnumerable<EventSeatDto> seats = new List<EventSeatDto>
            {
                new EventSeatDto { Id = 1, EventAreaId = 1, Row = 1, Number = 1 },
            };
            return seats;
        }

        [Test]
        public void DeleteEventSeat_WhenItIsntFree_ShouldReturnInvalidOperationException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                State = PlaceStatus.Occupied,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(testAction);
        }

        [Test]
        public void CreateEventSeat_WhenRowAndNumberArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Row = 0,
                Number = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventSeat_WhenRowAndNumberArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Row = 0,
                Number = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEventSeat_WhenRowAndNumberArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Id = 2,
                EventAreaId = 1,
                Row = 1,
                Number = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventSeat_WhenRowAndNumberArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Id = 2,
                EventAreaId = 1,
                Row = 1,
                Number = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
