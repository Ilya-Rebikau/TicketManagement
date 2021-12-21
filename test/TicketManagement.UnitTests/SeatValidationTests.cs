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
    internal class SeatValidationTests
    {
        private IService<Seat> _service;

        [SetUp]
        public void Setup()
        {
            var seatRepositoryMock = new Mock<IRepository<Seat>>();
            _service = new SeatService(seatRepositoryMock.Object);
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
            TestDelegate testAction = () => _service.Create(seat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
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
            TestDelegate testAction = () => _service.Update(seat);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
