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
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task CreateEventSeat__ShouldReturnAddedEventSeat()
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
            EventSeat addedEventSeat = await _service.CreateAsync(eventSeat);

            // Assert
            Assert.AreEqual(eventSeat, addedEventSeat);

            // DeleteAsync added event seat
            var eventSeats = await _service.GetAllAsync();
            await _service.DeleteAsync(eventSeats.Last());
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
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task UpdateEventSeat_WhenEventSeatDoesntExist_ShouldReturnEmptyEventSeat()
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
            EventSeat updatedSeat = await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.AreEqual(0, updatedSeat.Id);
        }

        [Test]
        public async Task UpdateEventSeat_ShouldReturnUpdatedEventSeat()
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
            EventSeat updatedEventSeat = await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.AreEqual(eventSeat, updatedEventSeat);

            // Back to old
            updatedEventSeat.Row = 1;
            await _service.UpdateAsync(updatedEventSeat);
        }

        [Test]
        public async Task GetEventSeatById_ShouldReturnFoundEventSeat()
        {
            // Arrange
            int id = 1;

            // Act
            EventSeat foundEventSeat = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundEventSeat.Id);
        }

        [Test]
        public async Task GetEventSeatById_WhenEventSeatDoesntExist_ShouldReturnEmptyEventSeat()
        {
            // Arrange
            int id = -1;

            // Act
            EventSeat eventSeat = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(0, eventSeat.Id);
        }
    }
}
