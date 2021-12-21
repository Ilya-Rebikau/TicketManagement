using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.UnitTests
{
    [TestFixture]
    internal class EventSeatValidationTests
    {
        private IService<EventSeat> _service;

        [SetUp]
        public void Setup()
        {
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            eventSeatRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestEventSeats());
            _service = new EventSeatService(eventSeatRepositoryMock.Object);
        }

        private IEnumerable<EventSeat> GetTestEventSeats()
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
            return seats;
        }

        [Test]
        public void CreateEventSeat_WhenRowAndNumberArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                EventAreaId = 1,
                Row = 0,
                Number = -1,
                State = 0,
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventSeat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventSeat_WhenRowAndNumberArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                Id = 1,
                EventAreaId = 1,
                Row = 0,
                Number = -1,
                State = 0,
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventSeat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEventSeat_WhenRowAndNumberArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                EventAreaId = 1,
                Row = 1,
                Number = 1,
                State = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventSeat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventSeat_WhenRowAndNumberArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                EventAreaId = 1,
                Row = 1,
                Number = 1,
                State = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventSeat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
