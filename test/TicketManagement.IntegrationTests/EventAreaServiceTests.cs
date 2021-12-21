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
    [TestFixture]
    internal class EventAreaServiceTests
    {
        private IService<EventArea> _service;

        [SetUp]
        public void Setup()
        {
            var eventAreaRepository = new EventAreaRepository();
            _service = new EventAreaService(eventAreaRepository);
        }

        [Test]
        public void CreateEventArea_WhenEventIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                EventId = 0,
                Description = "Description",
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventArea);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateEventArea_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                EventId = 1,
                Description = null,
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventArea);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateArea__ShouldReturnAddedArea()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                EventId = 1,
                Description = "Description",
                CoordX = 33123123,
                CoordY = 134123,
                Price = 1,
            };

            // Act
            EventArea addedEventArea = _service.Create(eventArea);

            // Assert
            Assert.AreEqual(eventArea, addedEventArea);

            // Delete added event area
            _service.Delete(_service.GetAll().Last());
        }

        [Test]
        public void UpdateEventArea_WhenLayoutIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                Id = 1,
                EventId = 0,
                Description = "Description",
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventArea);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                Id = 1,
                EventId = 1,
                Description = null,
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventArea);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenEventAreaDoesntExist_ShouldReturnEmptyEventArea()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                Id = 0,
                EventId = 1,
                Description = "Description",
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            EventArea updatedEventArea = _service.Update(eventArea);

            // Assert
            Assert.AreEqual(0, updatedEventArea.Id);
        }

        [Test]
        public void UpdateEventArea_ShouldReturnUpdatedArea()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                Id = 1,
                EventId = 1,
                Description = "First event area description",
                CoordX = 1,
                CoordY = 1,
                Price = 25,
            };

            // Act
            EventArea updatedEventArea = _service.Update(eventArea);

            // Assert
            Assert.AreEqual(eventArea, updatedEventArea);

            // Back to old
            updatedEventArea.Price = 11;
            _service.Update(updatedEventArea);
        }

        [Test]
        public void GetAreaById_ShouldReturnFoundArea()
        {
            // Arrange
            int id = 1;

            // Act
            EventArea foundEventArea = _service.GetById(id);

            // Assert
            Assert.AreEqual(id, foundEventArea.Id);
        }

        [Test]
        public void GetAreaById_WhenAreaDoesntExist_ShouldReturnEmptyArea()
        {
            // Arrange
            int id = -1;

            // Act
            EventArea eventArea = _service.GetById(id);

            // Assert
            Assert.AreEqual(0, eventArea.Id);
        }

        [Test]
        public void DeleteEventArea_WhenThereAreEventSeatsInIt_ShouldReturnSqlException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                Id = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Delete(eventArea);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }
    }
}
