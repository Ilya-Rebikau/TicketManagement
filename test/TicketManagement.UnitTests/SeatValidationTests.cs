using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.UnitTests
{
    [TestFixture]
    internal class SeatValidationTests
    {
        private IService<SeatDto> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var seatRepositoryMock = new Mock<IRepository<Seat>>();
            var seatConverterMock = new Mock<IConverter<Seat, SeatDto>>();
            seatRepositoryMock.Setup(rep => rep.GetAllAsync());
            var seats = await seatRepositoryMock.Object.GetAllAsync();
            seatConverterMock.Setup(rep => rep.ConvertModelsRangeToDtos(seats)).ReturnsAsync(GetTestSeatDtos());
            _service = new SeatService(seatRepositoryMock.Object, seatConverterMock.Object);
        }

        private static IEnumerable<SeatDto> GetTestSeatDtos()
        {
            IEnumerable<SeatDto> seats = new List<SeatDto>
            {
                new SeatDto { Id = 1, AreaId = 1, Row = 1, Number = 1 },
                new SeatDto { Id = 2, AreaId = 1, Row = 1, Number = 2 },
            };
            return seats;
        }

        [Test]
        public void CreateSeat_WhenRowAndNumberArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            SeatDto seat = new ()
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
            SeatDto seat = new ()
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
            SeatDto seat = new ()
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
            SeatDto seat = new ()
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
