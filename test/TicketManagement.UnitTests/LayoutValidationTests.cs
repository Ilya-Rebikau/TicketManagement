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
    internal class LayoutValidationTests
    {
        private IService<LayoutDto> _service;

        [SetUp]
        public void Setup()
        {
            var layoutRepositoryMock = new Mock<IRepository<Layout>>();
            var layoutConverterMock = new Mock<IConverter<Layout, LayoutDto>>();
            layoutRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestLayouts());
            var layouts = layoutRepositoryMock.Object.GetAll();
            layoutConverterMock.Setup(rep => rep.ConvertSourceModelRangeToDestinationModelRange(layouts)).ReturnsAsync(GetTestLayoutDtos());
            var eventRepositoryMock = new Mock<IRepository<Event>>();
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            var eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            var inMemorySettings = new Dictionary<string, string>
            {
                { "CountOnPage", "20" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            eventRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestEvents());
            eventAreaRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestEventAreas());
            eventSeatRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestEventSeats());
            _service = new LayoutService(layoutRepositoryMock.Object, layoutConverterMock.Object, eventRepositoryMock.Object,
                eventAreaRepositoryMock.Object, eventSeatRepositoryMock.Object, configuration);
        }

        private static IQueryable<Layout> GetTestLayouts()
        {
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, VenueId = 1, Name = "First layout", Description = "First layout description" },
                new Layout { Id = 2, VenueId = 1, Name = "Second layout", Description = "Second layout description" },
            };
            return layouts.AsQueryable();
        }

        private static IEnumerable<LayoutDto> GetTestLayoutDtos()
        {
            var layouts = new List<LayoutDto>
            {
                new LayoutDto { Id = 1, VenueId = 1, Name = "First layout", Description = "First layout description" },
                new LayoutDto { Id = 2, VenueId = 1, Name = "Second layout", Description = "Second layout description" },
            };
            return layouts;
        }

        private static IQueryable<Event> GetTestEvents()
        {
            var events = new List<Event>
            {
                new Event { Id = 1, LayoutId = 1 },
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

        [Test]
        public void DeleteLayout_WhenThereAreTicketsInIt_ShouldReturnValidationException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(layout);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateLayout_WhenNameIsntUnique_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenNameIsntUnique_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }
    }
}
