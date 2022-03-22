using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;
using TicketManagement.VenueManagerAPI.Services;

namespace TicketManagement.UnitTests
{
    internal class LayoutValidationTests
    {
        private IService<LayoutDto> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var layoutRepositoryMock = new Mock<IRepository<Layout>>();
            var layoutConverterMock = new Mock<IConverter<Layout, LayoutDto>>();
            layoutRepositoryMock.Setup(rep => rep.GetAllAsync());
            var layouts = await layoutRepositoryMock.Object.GetAllAsync();
            layoutConverterMock.Setup(rep => rep.ConvertModelsRangeToDtos(layouts)).ReturnsAsync(GetTestLayoutDtos());
            var eventRepositoryMock = new Mock<IRepository<Event>>();
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            eventRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEvents());
            eventAreaRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventAreas());
            eventSeatRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventSeats());
            _service = new LayoutService(layoutRepositoryMock.Object, layoutConverterMock.Object, eventRepositoryMock.Object,
                eventAreaRepositoryMock.Object, eventSeatRepositoryMock.Object);
        }

        private static IEnumerable<LayoutDto> GetTestLayoutDtos()
        {
            IEnumerable<LayoutDto> layouts = new List<LayoutDto>
            {
                new LayoutDto { Id = 1, VenueId = 1, Name = "First layout", Description = "First layout description" },
                new LayoutDto { Id = 2, VenueId = 1, Name = "Second layout", Description = "Second layout description" },
            };
            return layouts;
        }

        private static IQueryable<Event> GetTestEvents()
        {
            IEnumerable<Event> events = new List<Event>
            {
                new Event { Id = 1, LayoutId = 1 },
            };
            return events.AsQueryable();
        }

        private static IQueryable<EventArea> GetTestEventAreas()
        {
            IEnumerable<EventArea> eventAreas = new List<EventArea>
            {
                new EventArea { Id = 1, EventId = 1 },
            };
            return eventAreas.AsQueryable();
        }

        private static IQueryable<EventSeat> GetTestEventSeats()
        {
            IEnumerable<EventSeat> seats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, State = 1 },
            };
            return seats.AsQueryable();
        }

        [Test]
        public void DeleteLayout_WhenThereAreTicketsInIt_ShouldReturnInvalidOperationException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(layout);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(testAction);
        }

        [Test]
        public void CreateLayout_WhenNameIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                VenueId = 1,
                Name = "First layout",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenNameIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                Id = 2,
                VenueId = 1,
                Name = "First layout",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
