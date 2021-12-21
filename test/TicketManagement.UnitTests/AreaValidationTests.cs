using System;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.UnitTests
{
    internal class AreaValidationTests
    {
        private IService<Area> _service;

        [SetUp]
        public void Setup()
        {
            var areaRepositoryMock = new Mock<IRepository<Area>>();
            _service = new AreaService(areaRepositoryMock.Object);
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
    }
}
