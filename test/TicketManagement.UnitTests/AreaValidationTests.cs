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
    internal class AreaValidationTests
    {
        private IService<AreaDto> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var areaRepositoryMock = new Mock<IRepository<Area>>();
            var areaConverterMock = new Mock<IConverter<Area, AreaDto>>();
            areaRepositoryMock.Setup(rep => rep.GetAllAsync());
            var areas = await areaRepositoryMock.Object.GetAllAsync();
            areaConverterMock.Setup(rep => rep.ConvertModelsRangeToDtos(areas)).ReturnsAsync(GetTestAreaDtos());
            _service = new AreaService(areaRepositoryMock.Object, areaConverterMock.Object);
        }

        private static IEnumerable<AreaDto> GetTestAreaDtos()
        {
            IEnumerable<AreaDto> areas = new List<AreaDto>
            {
                new AreaDto { Id = 1, LayoutId = 1, Description = "First area of first layout", CoordX = 1, CoordY = 1, BasePrice = 11 },
                new AreaDto { Id = 2, LayoutId = 1, Description = "Second area of first layout", CoordX = 1, CoordY = 2, BasePrice = 12 },
                new AreaDto { Id = 3, LayoutId = 2, Description = "First area of second layout", CoordX = 1, CoordY = 1, BasePrice = 13 },
            };
            return areas;
        }

        [Test]
        public void CreateArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            AreaDto area = new ()
            {
                CoordX = -1,
                CoordY = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateArea_WhenBasePriceIsntPositive_ShouldReturnArgumentException()
        {
            // Arrange
            AreaDto area = new ()
            {
                BasePrice = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenCoordsArentPositive_ShouldReturnArgumentException()
        {
            // Arrange
            AreaDto area = new ()
            {
                CoordX = -1,
                CoordY = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenBasePriceIsntPositive_ShouldReturnArgumentException()
        {
            // Arrange
            AreaDto area = new ()
            {
                BasePrice = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateArea_WhenDescriptionIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            AreaDto area = new ()
            {
                LayoutId = 1,
                Description = "First area of first layout",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void CreateArea_WhenCoordsArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            AreaDto area = new ()
            {
                LayoutId = 1,
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenDescriptionIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            AreaDto area = new ()
            {
                LayoutId = 1,
                Description = "Second area of first layout",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenCoordsArentUnique_ShouldReturnArgumentException()
        {
            // Arrange
            AreaDto area = new ()
            {
                LayoutId = 1,
                CoordX = 1,
                CoordY = 2,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
