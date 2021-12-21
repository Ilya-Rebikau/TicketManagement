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
    internal class AreaValidationTests
    {
        private IService<Area> _service;

        [SetUp]
        public void Setup()
        {
            var areaRepositoryMock = new Mock<IRepository<Area>>();
            areaRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestAreas());
            _service = new AreaService(areaRepositoryMock.Object);
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

        [Test]
        public void CreateArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new ()
            {
                LayoutId = 1,
                Description = "Description",
                CoordX = -1,
                CoordY = 0,
            };

            // Act
            TestDelegate testAction = () => _service.Create(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new ()
            {
                Id = 1,
                LayoutId = 1,
                Description = "Description",
                CoordX = -1,
                CoordY = 0,
            };

            // Act
            TestDelegate testAction = () => _service.Update(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void CreateArea_WhenDescriptionIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new ()
            {
                LayoutId = 1,
                Description = "First area of first layout",
                CoordX = 10000,
                CoordY = 10000,
            };

            // Act
            TestDelegate testAction = () => _service.Create(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void CreateArea_WhenCoordsArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new ()
            {
                LayoutId = 1,
                Description = "Description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Create(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenDescriptionIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new ()
            {
                Id = 1,
                LayoutId = 1,
                Description = "Second area of first layout",
                CoordX = 10000,
                CoordY = 10000,
            };

            // Act
            TestDelegate testAction = () => _service.Update(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenCoordsArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Area area = new ()
            {
                Id = 1,
                LayoutId = 1,
                Description = "Description",
                CoordX = 1,
                CoordY = 2,
            };

            // Act
            TestDelegate testAction = () => _service.Update(area);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}
