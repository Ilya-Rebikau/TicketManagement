using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Infrastructure;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;
using TicketManagement.VenueManagerAPI.Services;

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
            var inMemorySettings = new Dictionary<string, string>
            {
                { "CountOnPage", "20" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            areaRepositoryMock.Setup(rep => rep.GetAllAsync());
            var areas = await areaRepositoryMock.Object.GetAllAsync();
            areaConverterMock.Setup(rep => rep.ConvertSourceModelRangeToDestinationModelRange(areas)).ReturnsAsync(GetTestAreaDtos());
            _service = new AreaService(areaRepositoryMock.Object, areaConverterMock.Object, configuration);
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
        public void CreateArea_WhenCoordsArentPositive_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateArea_WhenBasePriceIsntPositive_ShouldReturnValidationException()
        {
            // Arrange
            AreaDto area = new ()
            {
                BasePrice = -1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenCoordsArentPositive_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenBasePriceIsntPositive_ShouldReturnValidationException()
        {
            // Arrange
            AreaDto area = new ()
            {
                BasePrice = 0,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateArea_WhenDescriptionIsntUnique_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateArea_WhenCoordsArentUnique_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenDescriptionIsntUnique_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenCoordsArentUnique_ShouldReturnValidationException()
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
            Assert.ThrowsAsync<ValidationException>(testAction);
        }
    }
}
