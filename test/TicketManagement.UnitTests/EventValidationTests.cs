using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
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
            var usersClientMock = new Mock<IUsersClient>();
            var eventRepositoryMock = new Mock<IRepository<Event>>();
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            var eventConverterMock = new Mock<IConverter<Event, EventDto>>();
            var eventSeatServiceMock = new Mock<IService<EventSeatDto>>();
            var eventAreaServiceMock = new Mock<IService<EventAreaDto>>();
            eventRepositoryMock.Setup(rep => rep.GetAllAsync());
            eventSeatRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventSeats());
            eventAreaRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventAreas());
            var events = await eventRepositoryMock.Object.GetAllAsync();
            eventConverterMock.Setup(rep => rep.ConvertModelsRangeToDtos(events)).ReturnsAsync(GetTestEventDtos());
            _service = new EventService(eventRepositoryMock.Object, eventConverterMock.Object, eventAreaRepositoryMock.Object,
                eventSeatRepositoryMock.Object, eventAreaServiceMock.Object, eventSeatServiceMock.Object, usersClientMock.Object);
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
        public void CreateEvent_WhenEventAreasWithoutPrice_ShouldReturnInvalidOperationException()
        {
            // Arrange
            EventDto @event = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(@event);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void DeleteEvent_WhenThereAreTicketsInIt_ShouldReturnInvalidOperationException()
        {
            // Arrange
            EventDto @event = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(@event);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenTimeIsBusy_ShouldReturnArgumentException()
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
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenDatesInPast_ShouldReturnArgumentException()
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
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenWrongDates_ShouldReturnArgumentException()
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
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenDatesInPast_ShouldReturnArgumentException()
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
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenWrongDates_ShouldReturnArgumentException()
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
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
