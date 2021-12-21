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
            TestDelegate testAction = () => _service.Create(seat);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateSeat__ShouldReturnAddedSeat()
        {
            // Arrange
            Seat seat = new ()
            {
                AreaId = 1,
                Row = 12435212,
                Number = 123123,
            };

            // Act
            Seat addedSeat = _service.Create(seat);

            // Assert
            Assert.AreEqual(seat, addedSeat);

            // Delete added seat
            _service.Delete(_service.GetAll().Last());
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
            TestDelegate testAction = () => _service.Update(seat);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateSeat_WhenSeatDoesntExist_ShouldReturnEmptySeat()
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
            Seat updatedSeat = _service.Update(seat);

            // Assert
            Assert.AreEqual(0, updatedSeat.Id);
        }

        [Test]
        public void UpdateSeat_ShouldReturnUpdatedSeat()
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
            Seat updatedSeat = _service.Update(seat);

            // Assert
            Assert.AreEqual(seat, updatedSeat);

            // Back to old
            updatedSeat.Row = 1;
            _service.Update(updatedSeat);
        }

        [Test]
        public void GetSeatById_ShouldReturnFoundSeat()
        {
            // Arrange
            int id = 1;

            // Act
            Seat foundSeat = _service.GetById(id);

            // Assert
            Assert.AreEqual(id, foundSeat.Id);
        }

        [Test]
        public void GetSeatById_WhenSeatDoesntExist_ShouldReturnEmptySeat()
        {
            // Arrange
            int id = -1;

            // Act
            Seat seat = _service.GetById(id);

            // Assert
            Assert.AreEqual(0, seat.Id);
        }
    }
}
