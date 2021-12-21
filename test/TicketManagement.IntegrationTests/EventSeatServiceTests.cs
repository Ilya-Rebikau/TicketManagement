using System;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.IntegrationTests
{
    internal class EventSeatServiceTests
    {
        private IService<EventSeat> _service;

        [SetUp]
        public void Setup()
        {
            var eventSeatRepository = new EventSeatRepository();
            _service = new EventSeatService(eventSeatRepository);
        }

        [Test]
        public void CreateEventSeat_WhenEventAreaIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                EventAreaId = 0,
                Row = 12312412,
                Number = 1234124,
                State = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventSeat);

            // Assert
            Assert.Throws<SqlException>(testAction);
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
        public void CreateEventSeat__ShouldReturnAddedEventSeat()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                EventAreaId = 1,
                Row = 12435212,
                Number = 123123,
                State = 1,
            };

            // Act
            EventSeat addedEventSeat = _service.Create(eventSeat);

            // Assert
            Assert.AreEqual(eventSeat, addedEventSeat);

            // Delete added event seat
            _service.Delete(_service.GetAll().Last());
        }

        [Test]
        public void UpdateEventSeat_WhenEventAreaIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                Id = 1,
                EventAreaId = 0,
                Row = 12312412,
                Number = 1234124,
                State = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventSeat);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateEventSeat_WhenEventSeatDoesntExist_ShouldReturnEmptyEventSeat()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                Id = 0,
                EventAreaId = 1,
                Row = 12312412,
                Number = 1234124,
                State = 1,
            };

            // Act
            EventSeat updatedSeat = _service.Update(eventSeat);

            // Assert
            Assert.AreEqual(0, updatedSeat.Id);
        }

        [Test]
        public void UpdateEventSeat_ShouldReturnUpdatedEventSeat()
        {
            // Arrange
            EventSeat eventSeat = new ()
            {
                Id = 1,
                EventAreaId = 1,
                Row = 21234142,
                Number = 1,
                State = 1,
            };

            // Act
            EventSeat updatedEventSeat = _service.Update(eventSeat);

            // Assert
            Assert.AreEqual(eventSeat, updatedEventSeat);

            // Back to old
            updatedEventSeat.Row = 1;
            _service.Update(updatedEventSeat);
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

        [Test]
        public void GetEventSeatById_ShouldReturnFoundEventSeat()
        {
            // Arrange
            int id = 1;

            // Act
            EventSeat foundEventSeat = _service.GetById(id);

            // Assert
            Assert.AreEqual(id, foundEventSeat.Id);
        }

        [Test]
        public void GetEventSeatById_WhenEventSeatDoesntExist_ShouldReturnEmptyEventSeat()
        {
            // Arrange
            int id = -1;

            // Act
            EventSeat eventSeat = _service.GetById(id);

            // Assert
            Assert.AreEqual(0, eventSeat.Id);
        }
    }
}
