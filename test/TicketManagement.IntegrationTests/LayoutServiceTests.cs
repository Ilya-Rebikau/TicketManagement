using System;
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
    internal class LayoutServiceTests
    {
        private IService<Layout> _service;

        [SetUp]
        public void Setup()
        {
            var layoutRepository = new LayoutRepository();
            _service = new LayoutService(layoutRepository);
        }

        [Test]
        public void CreateLayout_WhenVenueIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Layout layout = new ()
            {
                VenueId = 0,
                Description = "Description",
                Name = "Name",
            };

            // Act
            TestDelegate testAction = () => _service.Create(layout);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateLayout_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Layout layout = new ()
            {
                VenueId = 1,
                Description = null,
                Name = "Name",
            };

            // Act
            TestDelegate testAction = () => _service.Create(layout);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateLayout_WhenNameIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Layout layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = null,
            };

            // Act
            TestDelegate testAction = () => _service.Create(layout);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateLayout__ShouldReturnAddedLayout()
        {
            // Arrange
            Layout layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = "Name",
            };

            // Act
            Layout addedLayout = _service.Create(layout);

            // Assert
            Assert.AreEqual(layout, addedLayout);

            // Delete added layout
            _service.Delete(_service.GetAll().Last());
        }

        [Test]
        public void UpdateLayout_WhenVenueIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Layout layout = new ()
            {
                Id = 1,
                VenueId = 0,
                Description = "Description",
                Name = "Name",
            };

            // Act
            TestDelegate testAction = () => _service.Update(layout);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Layout layout = new ()
            {
                Id = 1,
                VenueId = 1,
                Description = null,
                Name = "Name",
            };

            // Act
            TestDelegate testAction = () => _service.Update(layout);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenNameIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Layout layout = new ()
            {
                Id = 1,
                VenueId = 1,
                Description = "Description",
                Name = null,
            };

            // Act
            TestDelegate testAction = () => _service.Update(layout);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenLayoutDoesntExist_ShouldReturnEmptyLayout()
        {
            // Arrange
            Layout layout = new ()
            {
                Id = 0,
                VenueId = 1,
                Description = "Description",
                Name = "Name",
            };

            // Act
            Layout updatedLayout = _service.Update(layout);

            // Assert
            Assert.AreEqual(0, updatedLayout.Id);
        }

        [Test]
        public void UpdateLayout_ShouldReturnUpdatedLayout()
        {
            // Arrange
            Layout layout = new ()
            {
                Id = 1,
                VenueId = 1,
                Description = "New description",
                Name = "First layout",
            };

            // Act
            Layout updatedLayout = _service.Update(layout);

            // Assert
            Assert.AreEqual(layout, updatedLayout);

            // Back to old
            updatedLayout.Description = "First layout description";
            _service.Update(updatedLayout);
        }

        [Test]
        public void GetLayoutById_ShouldReturnFoundLayout()
        {
            // Arrange
            int id = 1;

            // Act
            Layout foundLayout = _service.GetById(id);

            // Assert
            Assert.AreEqual(id, foundLayout.Id);
        }

        [Test]
        public void GetLayoutById_WhenLayoutDoesntExist_ShouldReturnEmptyLayout()
        {
            // Arrange
            int id = -1;

            // Act
            Layout layout = _service.GetById(id);

            // Assert
            Assert.AreEqual(0, layout.Id);
        }

        [Test]
        public void CreateLayout_WhenNameIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Layout layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = "First layout",
            };

            // Act
            TestDelegate testAction = () => _service.Create(layout);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenNameIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Layout layout = new ()
            {
                Id = 2,
                VenueId = 1,
                Description = "Description",
                Name = "First layout",
            };

            // Act
            TestDelegate testAction = () => _service.Update(layout);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void DeleteLayout_WhenThereAreAreasInIt_ShouldReturnSqlException()
        {
            // Arrange
            Layout layout = new ()
            {
                Id = 1,
            };

            // Act
            TestDelegate testAction = () => _service.Delete(layout);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }
    }
}
