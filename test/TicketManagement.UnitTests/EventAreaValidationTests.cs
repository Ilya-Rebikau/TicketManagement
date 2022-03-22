using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class EventAreaValidationTests
    {
        private IService<EventAreaDto> _service;

        [SetUp]
        public void SetupAsync()
        {
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            var eventAreaConverterMock = new Mock<IConverter<EventArea, EventAreaDto>>();
            eventSeatRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventSeats());
            _service = new EventAreaService(eventAreaRepositoryMock.Object, eventAreaConverterMock.Object, eventSeatRepositoryMock.Object);
        }

        private static IQueryable<EventSeat> GetTestEventSeats()
        {
            IEnumerable<EventSeat> eventSeats = new List<EventSeat>
            {
                new EventSeat { EventAreaId = 1, State = 1 },
            };
            return eventSeats.AsQueryable();
        }

        [Test]
        public void DeleteEventArea_WhenThereAreTicketsInIt_ShouldReturnInvalidOperationException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(testAction);
        }

        [Test]
        public void CreateEventArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                CoordX = -1,
                CoordY = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                CoordX = -1,
                CoordY = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEventArea_WhenPriceArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Price = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenPriceArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Price = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
