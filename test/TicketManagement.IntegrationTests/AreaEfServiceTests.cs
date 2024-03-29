﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TicketManagement.DataAccess;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.RepositoriesEf;
using TicketManagement.VenueManagerAPI.Automapper;
using TicketManagement.VenueManagerAPI.Infrastructure;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;
using TicketManagement.VenueManagerAPI.Services;

namespace TicketManagement.IntegrationTests
{
    [TestFixture]
    internal class AreaEfServiceTests
    {
        private IService<AreaDto> _service;
        private IService<SeatDto> _seatService;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<TicketManagementContext>();

            builder.UseSqlServer(DbConnection.GetStringConnection())
                    .UseInternalServiceProvider(serviceProvider);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = config.CreateMapper();
            var context = new TicketManagementContext(builder.Options);
            var areaRepository = new AreaEfRepository(context);
            var areaConverter = new ModelsConverter<Area, AreaDto>(mapper);
            var configuration = new ConfigurationManager();
            configuration.AddJsonFile("appsettings.json");
            _service = new AreaService(areaRepository, areaConverter, configuration);
            var seatRepository = new EfRepository<Seat>(context);
            var seatConverter = new ModelsConverter<Seat, SeatDto>(mapper);
            _seatService = new SeatService(seatRepository, seatConverter, configuration);
        }

        [Test]
        public void CreateArea_WhenLayoutIdDoesntExist_ShouldReturnValidationException()
        {
            // Arrange
            AreaDto area = new ()
            {
                LayoutId = 0,
                Description = "Description",
                CoordX = 1,
                CoordY = 1,
                BasePrice = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateArea_WhenDescriptionIsNull_ShouldReturnValidationException()
        {
            // Arrange
            AreaDto area = new ()
            {
                LayoutId = 1,
                Description = null,
                CoordX = 100000000,
                CoordY = 100000000,
                BasePrice = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(area);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public async Task CreateArea__ShouldReturnAddedArea()
        {
            // Arrange
            AreaDto area = new ()
            {
                LayoutId = 1,
                Description = "Description",
                CoordX = 124325246,
                CoordY = 23452351,
                BasePrice = 1,
            };

            // Act
            AreaDto addedArea = await _service.CreateAsync(area);
            area.Id = addedArea.Id;

            // Assert
            area.Should().BeEquivalentTo(addedArea);

            // Delete added area
            await _service.DeleteAsync(addedArea);
        }

        [Test]
        public void UpdateArea_WhenLayoutIdDoesntExist_ShouldReturnValidationException()
        {
            // Arrange
            AreaDto area = new ()
            {
                Id = 1,
                LayoutId = 0,
                Description = "Description",
                CoordX = 1,
                CoordY = 1,
                BasePrice = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateArea_WhenDescriptionIsNull_ShouldReturnValidationException()
        {
            // Arrange
            AreaDto area = new ()
            {
                Id = 1,
                LayoutId = 1,
                Description = null,
                CoordX = 100000000,
                CoordY = 100000000,
                BasePrice = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(area);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public async Task UpdateArea_ShouldReturnUpdatedArea()
        {
            // Arrange
            AreaDto area = new ()
            {
                Id = 1,
                LayoutId = 1,
                Description = "New description!",
                CoordX = 1,
                CoordY = 1,
                BasePrice = 1,
            };

            // Act
            AreaDto updatedArea = await _service.UpdateAsync(area);

            // Assert
            area.Should().BeEquivalentTo(updatedArea);

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
            AreaDto foundArea = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundArea.Id);
        }

        [Test]
        public async Task DeleteArea_WhenThereAreSeatsInIt_ShouldDeletedSeats()
        {
            // Arrange
            AreaDto area = new ()
            {
                LayoutId = 1,
                BasePrice = 11,
                CoordX = 111111111,
                CoordY = 111111111,
                Description = "New description",
            };
            var addedArea = await _service.CreateAsync(area);
            var allSeats = await _seatService.GetAllAsync(1);
            int allSeatsCount = allSeats.Count();
            SeatDto seat = new ()
            {
                AreaId = addedArea.Id,
                Row = 1,
                Number = 1,
            };
            await _seatService.CreateAsync(seat);

            // Act
            await _service.DeleteAsync(addedArea);
            var allNewSeats = await _seatService.GetAllAsync(1);
            int allNewSeatsCount = allNewSeats.Count();

            // Assert
            Assert.AreEqual(allSeatsCount, allNewSeatsCount);
        }
    }
}
