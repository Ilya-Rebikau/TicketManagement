using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.UnitTests
{
    [TestFixture]
    internal class EventAreaValidationTests
    {
        private IService<EventArea> _service;

        [SetUp]
        public void Setup()
        {
            var eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            eventAreaRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestEventAreas());
            _service = new EventAreaService(eventAreaRepositoryMock.Object);
        }

        private IEnumerable<EventArea> GetTestEventAreas()
        {
            IEnumerable<EventArea> eventAreas = new List<EventArea>
            {
                new EventArea { Id = 1, EventId = 1, Description = "First event area description", CoordX = 1, CoordY = 1, Price = 11 },
                new EventArea { Id = 2, EventId = 1, Description = "Second event area description", CoordX = 1, CoordY = 2, Price = 12 },
            };
            return eventAreas;
        }

        [Test]
        public void CreateEventArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                EventId = 1,
                Description = "Description",
                CoordX = -1,
                CoordY = 0,
                Price = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventArea);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                Id = 1,
                EventId = 1,
                Description = "Description",
                CoordX = -1,
                CoordY = 0,
                Price = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventArea);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEventArea_WhenPriceArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                EventId = 1,
                Description = "Description",
                CoordX = 124214,
                CoordY = 12321421,
                Price = -1,
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventArea);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenPriceArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            EventArea eventArea = new ()
            {
                Id = 1,
                EventId = 1,
                Description = "Description",
                CoordX = 124124124,
                CoordY = 1241241243,
                Price = 0,
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventArea);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
