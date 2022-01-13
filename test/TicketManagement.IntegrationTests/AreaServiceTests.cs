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
                Description = "Description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task CreateArea__ShouldReturnAddedArea()
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
            Area addedArea = await _service.CreateAsync(area);

            // Assert
            Assert.AreEqual(area, addedArea);

            // DeleteAsync added area
            var areas = await _service.GetAllAsync();
            await _service.DeleteAsync(areas.Last());
        }

        [Test]
        public void UpdateArea_WhenLayoutIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            Area area = new ()
            {
                Id = 1,
                LayoutId = 0,
                Description = "Description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
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
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task UpdateArea_WhenAreaDoesntExist_ShouldReturnEmptyArea()
        {
            // Arrange
            Area area = new ()
            {
                Id = 0,
                LayoutId = 1,
                Description = "Description",
                CoordX = 1123124124,
                CoordY = 2123123123,
            };

            // Act
            Area updatedArea = await _service.UpdateAsync(area);

            // Assert
            Assert.AreEqual(0, updatedArea.Id);
        }

        [Test]
        public async Task UpdateArea_ShouldReturnUpdatedArea()
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
            Area updatedArea = await _service.UpdateAsync(area);

            // Assert
            Assert.AreEqual(area, updatedArea);

            // Back to old
            updatedArea.Description = "First area of first layout";
            await _service.UpdateAsync(updatedArea);
        }

        [Test]
        public async Task GetAreaById_ShouldReturnFoundArea()
        {
            // Arrange
            int id = 1;

            // Act
            Area foundArea = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundArea.Id);
        }

        [Test]
        public async Task GetAreaById_WhenAreaDoesntExist_ShouldReturnEmptyArea()
        {
            // Arrange
            int id = -1;

            // Act
            Area area = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(0, area.Id);
        }

        [Test]
        public void DeleteArea_WhenThereAreSeatsInIt_ShouldReturnSqlException()
        {
            // Arrange
            Area area = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(area);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }
    }
}
