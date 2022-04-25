using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Infrastructure;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;
using TicketManagement.EventManagerAPI.Services;

namespace TicketManagement.UnitTests
{
    [TestFixture]
    internal class EventSeatValidationTests
    {
        private IService<EventSeatDto> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            var eventSeatConverterMock = new Mock<IConverter<EventSeat, EventSeatDto>>();
            var inMemorySettings = new Dictionary<string, string>
            {
                { "CountOnPage", "20" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            eventSeatRepositoryMock.Setup(rep => rep.GetAllAsync());
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            var eventSeats = await eventSeatRepositoryMock.Object.GetAllAsync();
            eventSeatConverterMock.Setup(rep => rep.ConvertSourceModelRangeToDestinationModelRange(eventSeats)).ReturnsAsync(GetTestEventSeatDtos());
            eventSeatRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventSeat());
            _service = new EventSeatService(eventSeatRepositoryMock.Object, eventSeatConverterMock.Object, configuration, eventAreaRepositoryMock.Object);
        }

        private static IQueryable<EventSeat> GetTestEventSeat()
        {
            IEnumerable<EventSeat> seats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, Row = 1, Number = 1 },
            };
            return seats.AsQueryable();
        }

        private static IEnumerable<EventSeatDto> GetTestEventSeatDtos()
        {
            IEnumerable<EventSeatDto> seats = new List<EventSeatDto>
            {
                new EventSeatDto { Id = 1, EventAreaId = 1, Row = 1, Number = 1 },
            };
            return seats;
        }

        [Test]
        public void DeleteEventSeat_WhenItIsntFree_ShouldReturnValidationException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                State = PlaceStatus.Occupied,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateEventSeat_WhenRowAndNumberArentPositive_ShouldReturnValidationException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Row = 0,
                Number = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateEventSeat_WhenRowAndNumberArentPositive_ShouldReturnValidationException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Row = 0,
                Number = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateEventSeat_WhenRowAndNumberArentUnique_ShouldReturnValidationException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Id = 2,
                EventAreaId = 1,
                Row = 1,
                Number = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateEventSeat_WhenRowAndNumberArentUnique_ShouldReturnValidationException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Id = 2,
                EventAreaId = 1,
                Row = 1,
                Number = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }
    }
}
