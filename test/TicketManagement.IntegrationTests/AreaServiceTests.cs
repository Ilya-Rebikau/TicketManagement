using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.IntegrationTests
{
    [TestFixture]
    internal class AreaServiceTests
    {
        private IService<Area> _service;

        [SetUp]
        public void Setup()
        {
            var areaRepository = new AreaRepository();
            _service = new AreaService(areaRepository);
        }

        [Test]
        public void CreateArea_WhenLayoutIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Area area = new ()
            {
                LayoutId = 0,
                Description = "qweryuiopsadfgxflmgwekefmwad",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Create(area);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateArea_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Area area = new ()
            {
                LayoutId = 1,
                Description = null,
                CoordX = 100000000,
                CoordY = 100000000,
            };

            // Act
            TestDelegate testAction = () => _service.Create(area);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateArea__ShouldReturnAddedArea()
        {
            // Arrange
            Area area = new ()
            {
                LayoutId = 1,
                Description = "Description",
                CoordX = 124325246,
                CoordY = 23452351,
            };

            // Act
            Area addedArea = _service.Create(area);

            // Assert
            Assert.AreEqual(area, addedArea);

            // Delete added area
            _service.Delete(_service.GetAll().Last());
        }

        [Test]
        public void UpdateArea_WhenLayoutIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Area area = new ()
            {
                Id = 1,
                LayoutId = 0,
                Description = "qweryuiopsadfgxflmgwekefmwad",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Update(area);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Area area = new ()
            {
                Id = 1,
                LayoutId = 1,
                Description = null,
                CoordX = 100000000,
                CoordY = 100000000,
            };

            // Act
            TestDelegate testAction = () => _service.Update(area);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenAreaDoesntExist_ShouldReturnEmptyArea()
        {
            // Arrange
            Area area = new ()
            {
                Id = 0,
                LayoutId = 1,
                Description = "qweryuiopsadfgxflmgwekefmwad",
                CoordX = 1123124124,
                CoordY = 2123123123,
            };

            // Act
            Area updatedArea = _service.Update(area);

            // Assert
            Assert.AreEqual(0, updatedArea.Id);
        }

        [Test]
        public void UpdateArea_ShouldReturnUpdatedArea()
        {
            // Arrange
            Area area = new ()
            {
                Id = 1,
                LayoutId = 1,
                Description = "New description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            Area updatedArea = _service.Update(area);

            // Assert
            Assert.AreEqual(area, updatedArea);

            // Back to old
            updatedArea.Description = "First area of first layout";
            _service.Update(updatedArea);
        }

        [Test]
        public void GetAreaById_ShouldReturnFoundArea()
        {
            // Arrange
            int id = 1;

            // Act
            Area foundArea = _service.GetById(id);

            // Assert
            Assert.AreEqual(id, foundArea.Id);
        }

        [Test]
        public void GetAreaById_WhenAreaDoesntExist_ShouldReturnEmptyArea()
        {
            // Arrange
            int id = -1;

            // Act
            Area area = _service.GetById(id);

            // Assert
            Assert.AreEqual(0, area.Id);
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
                Description = "qweryuiopsadfgxflmgwekefmwad",
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
                Description = "qweryuiopsadfgxflmgwekefmwad",
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
