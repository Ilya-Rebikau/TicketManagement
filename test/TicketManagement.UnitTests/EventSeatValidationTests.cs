using System;
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
            _service = new EventSeatService(eventSeatRepositoryMock.Object);
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
    }
}
