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
    internal class EventServiceTests
    {
        private IService<Event> _service;

        [SetUp]
        public void Setup()
        {
            var eventRepository = new EventRepository();
            var seatRepository = new SeatRepository();
            var areaRepository = new AreaRepository();
            var layoutRepository = new LayoutRepository();
            _service = new EventService(eventRepository, seatRepository, areaRepository, layoutRepository);
        }

        [Test]
        public void CreateEvent_WhenTimeIsBusy_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Name = "Name",
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2030, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2030, 12, 21, 16, 40, 0),
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventModel);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenNameIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Name = null,
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventModel);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Name = "Name",
                Description = null,
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventModel);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenLayoudIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Name = "Name",
                Description = "Description",
                LayoutId = 0,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventModel);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateEvent__ShouldReturnAddedEvent()
        {
            // Arrange
            Event eventModel = new ()
            {
                Name = "Name",
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            Event addedEvent = _service.Create(eventModel);

            // Assert
            Assert.AreEqual(eventModel, addedEvent);

            // Delete added event
            _service.Delete(_service.GetAll().Last());
        }

        [Test]
        public void UpdateEvent_WhenNameIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Id = 1,
                Name = null,
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventModel);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Id = 1,
                Name = "Name",
                Description = null,
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventModel);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenLayoudIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                LayoutId = 0,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventModel);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateEvent_ShouldReturnUpdatedEvent()
        {
            // Arrange
            Event eventModel = new ()
            {
                Id = 1,
                Name = "New name",
                Description = "First event description",
                LayoutId = 1,
                TimeStart = new DateTime(2030, 12, 21, 15, 0, 0),
                TimeEnd = new DateTime(2030, 12, 21, 17, 0, 0),
            };

            // Act
            Event updatedEvent = _service.Update(eventModel);

            // Assert
            Assert.AreEqual(eventModel, updatedEvent);

            // Back to old
            updatedEvent.Name = "First event name";
            _service.Update(updatedEvent);
        }

        [Test]
        public void GetEventById_ShouldReturnFoundEvent()
        {
            // Arrange
            int id = 1;

            // Act
            Event foundEvent = _service.GetById(id);

            // Assert
            Assert.AreEqual(id, foundEvent.Id);
        }

        [Test]
        public void GetAreaById_WhenAreaDoesntExist_ShouldReturnEmptyArea()
        {
            // Arrange
            int id = -1;

            // Act
            Event foundEvent = _service.GetById(id);

            // Assert
            Assert.AreEqual(0, foundEvent.Id);
        }

        [Test]
        public void DeleteEvent_WhenThereAreEventAreasInIt_ShouldReturnSqlException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Id = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Delete(eventModel);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }
    }
}
