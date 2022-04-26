using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Infrastructure;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;
using TicketManagement.VenueManagerAPI.Services;

namespace TicketManagement.UnitTests
{
    [TestFixture]
    internal class SeatValidationTests
    {
        private IService<SeatDto> _service;

        [SetUp]
        public void Setup()
        {
            var seatRepositoryMock = new Mock<IRepository<Seat>>();
            var seatConverterMock = new Mock<IConverter<Seat, SeatDto>>();
            var inMemorySettings = new Dictionary<string, string>
            {
                { "CountOnPage", "20" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            seatRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestSeats());
            var seats = seatRepositoryMock.Object.GetAll();
            seatConverterMock.Setup(rep => rep.ConvertSourceModelRangeToDestinationModelRange(seats)).ReturnsAsync(GetTestSeatDtos());
            _service = new SeatService(seatRepositoryMock.Object, seatConverterMock.Object, configuration);
        }

        private static IQueryable<Seat> GetTestSeats()
        {
            var seats = new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Row = 1, Number = 1 },
                new Seat { Id = 2, AreaId = 1, Row = 1, Number = 2 },
            };
            return seats.AsQueryable();
        }

        private static IEnumerable<SeatDto> GetTestSeatDtos()
        {
            var seats = new List<SeatDto>
            {
                new SeatDto { Id = 1, AreaId = 1, Row = 1, Number = 1 },
                new SeatDto { Id = 2, AreaId = 1, Row = 1, Number = 2 },
            };
            return seats;
        }

        [Test]
        public void CreateSeat_WhenRowAndNumberArentUnique_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateSeat_WhenRowAndNumberArentUnique_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateSeat_WhenRowAndNumberArentPositive_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateSeat_WhenRowAndNumberArentPositive_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }
    }
}
