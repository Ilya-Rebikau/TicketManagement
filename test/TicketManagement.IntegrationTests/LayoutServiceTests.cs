using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(layout);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(layout);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(layout);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task CreateLayout__ShouldReturnAddedLayout()
        {
            // Arrange
            Layout layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = "Name",
            };

            // Act
            Layout addedLayout = await _service.CreateAsync(layout);

            // Assert
            Assert.AreEqual(layout, addedLayout);

            // DeleteAsync added layout
            var layouts = await _service.GetAllAsync();
            await _service.DeleteAsync(layouts.Last());
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
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(layout);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(layout);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(layout);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task UpdateLayout_WhenLayoutDoesntExist_ShouldReturnEmptyLayout()
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
            Layout updatedLayout = await _service.UpdateAsync(layout);

            // Assert
            Assert.AreEqual(0, updatedLayout.Id);
        }

        [Test]
        public async Task UpdateLayout_ShouldReturnUpdatedLayout()
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
            Layout updatedLayout = await _service.UpdateAsync(layout);

            // Assert
            Assert.AreEqual(layout, updatedLayout);

            // Back to old
            updatedLayout.Description = "First layout description";
            await _service.UpdateAsync(updatedLayout);
        }

        [Test]
        public async Task GetLayoutById_ShouldReturnFoundLayout()
        {
            // Arrange
            int id = 1;

            // Act
            Layout foundLayout = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundLayout.Id);
        }

        [Test]
        public async Task GetLayoutById_WhenLayoutDoesntExist_ShouldReturnEmptyLayout()
        {
            // Arrange
            int id = -1;

            // Act
            Layout layout = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(0, layout.Id);
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
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(layout);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }
    }
}
