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
    internal class LayoutValidationTests
    {
        private IService<LayoutDto> _service;

        [SetUp]
        public async Task SetupAsync()
        {
            var layoutRepositoryMock = new Mock<IRepository<Layout>>();
            var layoutConverterMock = new Mock<IConverter<Layout, LayoutDto>>();
            layoutRepositoryMock.Setup(rep => rep.GetAllAsync()).ReturnsAsync(GetTestLayouts());
            var layouts = await layoutRepositoryMock.Object.GetAllAsync();
            layoutConverterMock.Setup(rep => rep.ConvertModelsRangeToDtos(layouts)).ReturnsAsync(GetTestLayoutDtos());
            _service = new LayoutService(layoutRepositoryMock.Object, layoutConverterMock.Object);
        }

        private static IEnumerable<LayoutDto> GetTestLayoutDtos()
        {
            IEnumerable<LayoutDto> layouts = new List<LayoutDto>
            {
                new LayoutDto { Id = 1, VenueId = 1, Name = "First layout", Description = "First layout description" },
                new LayoutDto { Id = 2, VenueId = 1, Name = "Second layout", Description = "Second layout description" },
            };
            return layouts;
        }

        private static IQueryable<Layout> GetTestLayouts()
        {
            IEnumerable<Layout> layouts = new List<Layout>
            {
                new Layout { Id = 1, VenueId = 1, Name = "First layout", Description = "First layout description" },
                new Layout { Id = 2, VenueId = 1, Name = "Second layout", Description = "Second layout description" },
            };
            return layouts.AsQueryable();
        }

        [Test]
        public void CreateLayout_WhenNameIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = "First layout",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenNameIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                Id = 2,
                VenueId = 1,
                Description = "Description",
                Name = "First layout",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(testAction);
        }
    }
}
