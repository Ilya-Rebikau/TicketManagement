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
            eventRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestEvents());
            seatRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestSeats());
            areaRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestAreas());
            layoutRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestLayouts());
            _service = new EventService(eventRepositoryMock.Object, seatRepositoryMock.Object, areaRepositoryMock.Object, layoutRepositoryMock.Object);
        }

        private IEnumerable<Seat> GetTestSeats()
        {
            IEnumerable<Seat> seats = new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Row = 1, Number = 1 },
                new Seat { Id = 2, AreaId = 1, Row = 1, Number = 2 },
                new Seat { Id = 3, AreaId = 1, Row = 1, Number = 3 },
                new Seat { Id = 4, AreaId = 1, Row = 2, Number = 1 },
                new Seat { Id = 5, AreaId = 1, Row = 2, Number = 2 },
                new Seat { Id = 6, AreaId = 1, Row = 2, Number = 3 },
                new Seat { Id = 7, AreaId = 2, Row = 1, Number = 1 },
                new Seat { Id = 8, AreaId = 2, Row = 1, Number = 2 },
                new Seat { Id = 9, AreaId = 2, Row = 1, Number = 3 },
                new Seat { Id = 10, AreaId = 2, Row = 2, Number = 1 },
                new Seat { Id = 11, AreaId = 2, Row = 2, Number = 2 },
                new Seat { Id = 12, AreaId = 2, Row = 2, Number = 3 },
                new Seat { Id = 13, AreaId = 3, Row = 1, Number = 1 },
                new Seat { Id = 14, AreaId = 3, Row = 1, Number = 2 },
                new Seat { Id = 15, AreaId = 3, Row = 1, Number = 3 },
                new Seat { Id = 16, AreaId = 3, Row = 2, Number = 1 },
                new Seat { Id = 17, AreaId = 3, Row = 2, Number = 2 },
                new Seat { Id = 18, AreaId = 3, Row = 2, Number = 3 },
            };
            return seats;
        }

        private IEnumerable<Area> GetTestAreas()
        {
            IEnumerable<Area> areas = new List<Area>
            {
                new Area { Id = 1, LayoutId = 1, Description = "First area of first layout", CoordX = 1, CoordY = 1 },
                new Area { Id = 2, LayoutId = 1, Description = "Second area of first layout", CoordX = 1, CoordY = 2 },
                new Area { Id = 3, LayoutId = 2, Description = "First area of second layout", CoordX = 1, CoordY = 1 },
            };
            return areas;
        }

        private IEnumerable<Event> GetTestEvents()
        {
            IEnumerable<Event> events = new List<Event>
            {
                new Event
                {
                    Id = 1, Name = "First event name", Description = "First event description", LayoutId = 1,
                    TimeStart = new DateTime(2030, 12, 21, 15, 0, 0), TimeEnd = new DateTime(2030, 12, 21, 17, 0, 0),
                },
            };
            return events;
        }

        private IEnumerable<Layout> GetTestLayouts()
        {
            IEnumerable<Layout> layouts = new List<Layout>
            {
                new Layout { Id = 1, VenueId = 1, Name = "First layout", Description = "First layout description" },
                new Layout { Id = 2, VenueId = 1, Name = "Second layout", Description = "Second layout description" },
            };
            return layouts;
        }

        [Test]
        public void CreateEvent_WhenTimeIsBusy_ShouldReturnArgumentException()
        {
            // Arrange
            Event eventModel = new ()
            {
                Name = "Name",
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2030, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2030, 12, 21, 16, 40, 0),
            };

            // Act
            TestDelegate testAction = () => _service.Create(eventModel);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
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
