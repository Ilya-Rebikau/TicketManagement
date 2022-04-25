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
    internal class EventValidationTests
    {
        private IService<EventDto> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var eventRepositoryMock = new Mock<IRepository<Event>>();
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            var eventConverterMock = new Mock<IConverter<Event, EventDto>>();
            var inMemorySettings = new Dictionary<string, string>
            {
                { "CountOnPage", "20" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            eventRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEvents());
            eventSeatRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventSeats());
            eventAreaRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventAreas());
            var events = await eventRepositoryMock.Object.GetAllAsync();
            eventConverterMock.Setup(rep => rep.ConvertSourceModelRangeToDestinationModelRange(events)).ReturnsAsync(GetTestEventDtos());
            _service = new EventCrudService(eventRepositoryMock.Object, eventConverterMock.Object, eventAreaRepositoryMock.Object,
                eventSeatRepositoryMock.Object, configuration);
        }

        private static IQueryable<Event> GetTestEvents()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1, Name = "First event name", Description = "First event description", LayoutId = 1,
                    TimeStart = new DateTime(2030, 12, 21, 15, 0, 0), TimeEnd = new DateTime(2030, 12, 21, 17, 0, 0),
                    ImageUrl = "https://w-dog.ru/wallpapers/5/16/428743654433638/kotyata-serye-zhivotnye-trava-gazon.jpg",
                },
            };

            return events.AsQueryable();
        }

        private static IQueryable<EventArea> GetTestEventAreas()
        {
            var eventAreas = new List<EventArea>
            {
                new EventArea { Id = 1, EventId = 1 },
            };
            return eventAreas.AsQueryable();
        }

        private static IQueryable<EventSeat> GetTestEventSeats()
        {
            var seats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, State = 1 },
            };
            return seats.AsQueryable();
        }

        private static IEnumerable<EventDto> GetTestEventDtos()
        {
            var events = new List<EventDto>
            {
                new EventDto
                {
                    Id = 1, Name = "First event name", Description = "First event description", LayoutId = 1,
                    TimeStart = new DateTime(2030, 12, 21, 15, 0, 0), TimeEnd = new DateTime(2030, 12, 21, 17, 0, 0),
                    ImageUrl = "https://w-dog.ru/wallpapers/5/16/428743654433638/kotyata-serye-zhivotnye-trava-gazon.jpg",
                },
            };
            return events;
        }

        [Test]
        public void CreateEvent_WhenEventAreasWithoutPrice_ShouldReturnValidationException()
        {
            // Arrange
            EventDto @event = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(@event);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void DeleteEvent_WhenThereAreTicketsInIt_ShouldReturnValidationException()
        {
            // Arrange
            EventDto @event = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(@event);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenTimeIsBusy_ShouldReturnValidationException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Id = 2,
                LayoutId = 1,
                TimeStart = new DateTime(2025, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2035, 12, 21, 16, 40, 0),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenDatesInPast_ShouldReturnValidationException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                TimeStart = new DateTime(2000, 1, 1),
                TimeEnd = new DateTime(2022, 2, 2),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenWrongDates_ShouldReturnValidationException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                TimeStart = new DateTime(2023, 1, 1),
                TimeEnd = new DateTime(2022, 2, 2),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenDatesInPast_ShouldReturnValidationException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                TimeStart = new DateTime(2000, 1, 1),
                TimeEnd = new DateTime(2022, 2, 2),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenWrongDates_ShouldReturnValidationException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                TimeStart = new DateTime(2023, 1, 1),
                TimeEnd = new DateTime(2022, 2, 2),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }
    }
}
