using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

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
            eventSeatRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventSeats());
            var eventSeats = await eventSeatRepositoryMock.Object.GetAllAsync();
            eventSeatConverterMock.Setup(rep => rep.ConvertModelsRangeToDtos(eventSeats)).ReturnsAsync(GetTestEventSeatDtos());
            _service = new EventSeatService(eventSeatRepositoryMock.Object, eventSeatConverterMock.Object);
        }

        private static IEnumerable<EventSeatDto> GetTestEventSeatDtos()
        {
            IEnumerable<EventSeatDto> seats = new List<EventSeatDto>
            {
                new EventSeatDto { Id = 1, EventAreaId = 1, Row = 1, Number = 1, State = 1 },
                new EventSeatDto { Id = 2, EventAreaId = 1, Row = 1, Number = 2, State = 1 },
                new EventSeatDto { Id = 3, EventAreaId = 1, Row = 2, Number = 1, State = 1 },
                new EventSeatDto { Id = 4, EventAreaId = 2, Row = 1, Number = 1, State = 0 },
                new EventSeatDto { Id = 5, EventAreaId = 2, Row = 1, Number = 2, State = 1 },
                new EventSeatDto { Id = 6, EventAreaId = 2, Row = 2, Number = 1, State = 0 },
            };
            return seats;
        }

        private static IQueryable<EventSeat> GetTestEventSeats()
        {
            IEnumerable<EventSeat> seats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, Row = 1, Number = 1, State = 1 },
                new EventSeat { Id = 2, EventAreaId = 1, Row = 1, Number = 2, State = 1 },
                new EventSeat { Id = 3, EventAreaId = 1, Row = 2, Number = 1, State = 1 },
                new EventSeat { Id = 4, EventAreaId = 2, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 5, EventAreaId = 2, Row = 1, Number = 2, State = 1 },
                new EventSeat { Id = 6, EventAreaId = 2, Row = 2, Number = 1, State = 0 },
            };
            return seats.AsQueryable();
        }

        [Test]
        public void CreateEventSeat_WhenRowAndNumberArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                EventAreaId = 1,
                Row = 0,
                Number = -1,
                State = 0,
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
                Id = 1,
                EventAreaId = 1,
                Row = 0,
                Number = -1,
                State = 0,
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
                EventAreaId = 1,
                Row = 1,
                Number = 1,
                State = 1,
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
                EventAreaId = 1,
                Row = 1,
                Number = 1,
                State = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
