using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task CreateEvent__ShouldReturnAddedEvent()
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
            Event addedEvent = await _service.CreateAsync(eventModel);

            // Assert
            Assert.AreEqual(eventModel, addedEvent);

            // DeleteAsync added event
            var events = await _service.GetAllAsync();
            await _service.DeleteAsync(events.Last());
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
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task UpdateEvent_ShouldReturnUpdatedEvent()
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
            Event updatedEvent = await _service.UpdateAsync(eventModel);

            // Assert
            Assert.AreEqual(eventModel, updatedEvent);

            // Back to old
            updatedEvent.Name = "First event name";
            await _service.UpdateAsync(updatedEvent);
        }

        [Test]
        public async Task GetEventById_ShouldReturnFoundEvent()
        {
            // Arrange
            int id = 1;

            // Act
            Event foundEvent = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundEvent.Id);
        }

        [Test]
        public async Task GetAreaById_WhenAreaDoesntExist_ShouldReturnEmptyArea()
        {
            // Arrange
            int id = -1;

            // Act
            Event foundEvent = await _service.GetByIdAsync(id);

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
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }
    }
}
