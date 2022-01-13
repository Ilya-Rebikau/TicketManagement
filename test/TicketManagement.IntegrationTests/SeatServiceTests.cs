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
    internal class SeatServiceTests
    {
        private IService<Seat> _service;

        [SetUp]
        public void Setup()
        {
            var seatRepository = new SeatRepository();
            _service = new SeatService(seatRepository);
        }

        [Test]
        public void CreateSeat_WhenAreaIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Seat seat = new ()
            {
                AreaId = 0,
                Row = 12312412,
                Number = 1234124,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(seat);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task CreateSeat__ShouldReturnAddedSeat()
        {
            // Arrange
            Seat seat = new ()
            {
                AreaId = 1,
                Row = 12435212,
                Number = 123123,
            };

            // Act
            Seat addedSeat = await _service.CreateAsync(seat);

            // Assert
            Assert.AreEqual(seat, addedSeat);

            // DeleteAsync added seat
            var seats = await _service.GetAllAsync();
            await _service.DeleteAsync(seats.Last());
        }

        [Test]
        public void UpdateSeat_WhenAreaIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Seat seat = new ()
            {
                Id = 1,
                AreaId = 0,
                Row = 12312412,
                Number = 1234124,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(seat);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task UpdateSeat_WhenSeatDoesntExist_ShouldReturnEmptySeat()
        {
            // Arrange
            Seat seat = new ()
            {
                Id = 0,
                AreaId = 1,
                Row = 115124,
                Number = 1231231,
            };

            // Act
            Seat updatedSeat = await _service.UpdateAsync(seat);

            // Assert
            Assert.AreEqual(0, updatedSeat.Id);
        }

        [Test]
        public async Task UpdateSeat_ShouldReturnUpdatedSeat()
        {
            // Arrange
            Seat seat = new ()
            {
                Id = 1,
                AreaId = 1,
                Row = 123123,
                Number = 1,
            };

            // Act
            Seat updatedSeat = await _service.UpdateAsync(seat);

            // Assert
            Assert.AreEqual(seat, updatedSeat);

            // Back to old
            updatedSeat.Row = 1;
            await _service.UpdateAsync(updatedSeat);
        }

        [Test]
        public async Task GetSeatById_ShouldReturnFoundSeat()
        {
            // Arrange
            int id = 1;

            // Act
            Seat foundSeat = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundSeat.Id);
        }

        [Test]
        public async Task GetSeatById_WhenSeatDoesntExist_ShouldReturnEmptySeat()
        {
            // Arrange
            int id = -1;

            // Act
            Seat seat = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(0, seat.Id);
        }
    }
}
