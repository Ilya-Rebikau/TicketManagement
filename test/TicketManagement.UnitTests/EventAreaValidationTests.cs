using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class EventAreaValidationTests
    {
        private IService<EventAreaDto> _service;

        [SetUp]
        public void SetupAsync()
        {
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            var eventAreaConverterMock = new Mock<IConverter<EventArea, EventAreaDto>>();
            var inMemorySettings = new Dictionary<string, string>
            {
                { "CountOnPage", "20" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            eventSeatRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestEventSeats());
            _service = new EventAreaService(eventAreaRepositoryMock.Object, eventAreaConverterMock.Object, eventSeatRepositoryMock.Object, configuration);
        }

        private static IQueryable<EventSeat> GetTestEventSeats()
        {
            IEnumerable<EventSeat> eventSeats = new List<EventSeat>
            {
                new EventSeat { EventAreaId = 1, State = 1 },
            };
            return eventSeats.AsQueryable();
        }

        [Test]
        public void DeleteEventArea_WhenThereAreTicketsInIt_ShouldReturnValidationException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateEventArea_WhenCoordsArentPositive_ShouldReturnValidationException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                CoordX = -1,
                CoordY = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenCoordsArentPositive_ShouldReturnValidationException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                CoordX = -1,
                CoordY = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateEventArea_WhenPriceArentPositive_ShouldReturnValidationException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Price = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenPriceArentPositive_ShouldReturnValidationException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Price = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }
    }
}
