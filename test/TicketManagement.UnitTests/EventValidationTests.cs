using System;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.UnitTests
{
    [TestFixture]
    internal class EventValidationTests
    {
        private IService<Event> _service;

        [SetUp]
        public void Setup()
        {
            var eventRepositoryMock = new Mock<IRepository<Event>>();
            var seatRepositoryMock = new Mock<IRepository<Seat>>();
            var areaRepositoryMock = new Mock<IRepository<Area>>();
            var layoutRepositoryMock = new Mock<IRepository<Layout>>();
            _service = new EventService(eventRepositoryMock.Object, seatRepositoryMock.Object, areaRepositoryMock.Object, layoutRepositoryMock.Object);
        }

        [Test]
        public void CreateEvent_WhenDatesInPast_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Name = "Name",
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2000, 1, 1),
                TimeEnd = new DateTime(2022, 2, 2),
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventModel);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenWrongDates_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Name = "Name",
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2023, 1, 1),
                TimeEnd = new DateTime(2022, 2, 2),
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventModel);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenDatesInPast_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2000, 1, 1),
                TimeEnd = new DateTime(2022, 2, 2),
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventModel);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenWrongDates_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2023, 1, 1),
                TimeEnd = new DateTime(2022, 2, 2),
            };

            // Act
            TestDelegate testAction = () => _service.Update(eventModel);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
