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
    internal class EventAreaValidationTests
    {
        private IService<EventAreaDto> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            var eventAreaConverterMock = new Mock<IConverter<EventArea, EventAreaDto>>();
            eventAreaRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestEventAreas());
            var eventAreas = await eventAreaRepositoryMock.Object.GetAllAsync();
            eventAreaConverterMock.Setup(rep => rep.ConvertModelsRangeToDtos(eventAreas)).ReturnsAsync(GetTestEventAreaDtos());
            _service = new EventAreaService(eventAreaRepositoryMock.Object, eventAreaConverterMock.Object);
        }

        private static IEnumerable<EventAreaDto> GetTestEventAreaDtos()
        {
            IEnumerable<EventAreaDto> eventAreas = new List<EventAreaDto>
            {
                new EventAreaDto { Id = 1, EventId = 1, Description = "First event area description", CoordX = 1, CoordY = 1, Price = 11 },
                new EventAreaDto { Id = 2, EventId = 1, Description = "Second event area description", CoordX = 1, CoordY = 2, Price = 12 },
            };
            return eventAreas;
        }

        private static IQueryable<EventArea> GetTestEventAreas()
        {
            IEnumerable<EventArea> eventAreas = new List<EventArea>
            {
                new EventArea { Id = 1, EventId = 1, Description = "First event area description", CoordX = 1, CoordY = 1, Price = 11 },
                new EventArea { Id = 2, EventId = 1, Description = "Second event area description", CoordX = 1, CoordY = 2, Price = 12 },
            };
            return eventAreas.AsQueryable();
        }

        [Test]
        public void CreateEventArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                EventId = 1,
                Description = "Description",
                CoordX = -1,
                CoordY = 0,
                Price = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Id = 1,
                EventId = 1,
                Description = "Description",
                CoordX = -1,
                CoordY = 0,
                Price = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEventArea_WhenPriceArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                EventId = 1,
                Description = "Description",
                CoordX = 124214,
                CoordY = 12321421,
                Price = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenPriceArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Id = 1,
                EventId = 1,
                Description = "Description",
                CoordX = 124124124,
                CoordY = 1241241243,
                Price = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
