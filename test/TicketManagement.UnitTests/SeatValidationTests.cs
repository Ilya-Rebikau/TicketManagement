using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.UnitTests
{
    [TestFixture]
    internal class SeatValidationTests
    {
        private IService<Seat> _service;

        [SetUp]
        public void Setup()
        {
            var seatRepositoryMock = new Mock<IRepository<Seat>>();
            seatRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestSeats());
            _service = new SeatService(seatRepositoryMock.Object);
        }

        private static IQueryable<Seat> GetTestSeats()
        {
            IEnumerable<Seat> seats = new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Row = 1, Number = 1 },
                new Seat { Id = 2, AreaId = 1, Row = 1, Number = 2 },
                new Seat { Id = 3, AreaId = 1, Row = 1, Number = 3 },
                new Seat { Id = 4, AreaId = 1, Row = 2, Number = 1 },
                new Seat { Id = 5, AreaId = 1, Row = 2, Number = 2 },
                new Seat { Id = 6, AreaId = 1, Row = 2, Number = 3 },
                new Seat { Id = 7, AreaId = 2, Row = 1, Number = 1 },
                new Seat { Id = 8, AreaId = 2, Row = 1, Number = 2 },
                new Seat { Id = 9, AreaId = 2, Row = 1, Number = 3 },
                new Seat { Id = 10, AreaId = 2, Row = 2, Number = 1 },
                new Seat { Id = 11, AreaId = 2, Row = 2, Number = 2 },
                new Seat { Id = 12, AreaId = 2, Row = 2, Number = 3 },
                new Seat { Id = 13, AreaId = 3, Row = 1, Number = 1 },
                new Seat { Id = 14, AreaId = 3, Row = 1, Number = 2 },
                new Seat { Id = 15, AreaId = 3, Row = 1, Number = 3 },
                new Seat { Id = 16, AreaId = 3, Row = 2, Number = 1 },
                new Seat { Id = 17, AreaId = 3, Row = 2, Number = 2 },
                new Seat { Id = 18, AreaId = 3, Row = 2, Number = 3 },
            };
            return seats.AsQueryable();
        }

        [Test]
        public void CreateSeat_WhenRowAndNumberArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Seat seat = new ()
            {
                AreaId = 1,
                Row = 1,
                Number = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(seat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateSeat_WhenRowAndNumberArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Seat seat = new ()
            {
                Id = 1,
                AreaId = 1,
                Row = 1,
                Number = 2,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(seat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateSeat_WhenRowAndNumberArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            Seat seat = new ()
            {
                AreaId = 1,
                Row = 0,
                Number = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(seat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateSeat_WhenRowAndNumberArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            Seat seat = new ()
            {
                Id = 1,
                AreaId = 1,
                Row = 0,
                Number = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(seat);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
