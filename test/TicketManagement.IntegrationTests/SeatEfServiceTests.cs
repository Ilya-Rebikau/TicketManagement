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
    internal class SeatEfServiceTests
    {
        private IService<SeatDto> _service;

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
            var configuration = new ConfigurationManager();
            configuration.AddJsonFile("appsettings.json");
            var context = new TicketManagementContext(builder.Options);
            var seatRepository = new EfRepository<Seat>(context);
            var seatConverter = new ModelsConverter<Seat, SeatDto>(mapper);
            _service = new SeatService(seatRepository, seatConverter, configuration);
        }

        [Test]
        public void CreateSeat_WhenAreaIdDoesntExist_ShouldReturnValidationException()
        {
            // Arrange
            SeatDto seat = new ()
            {
                AreaId = 0,
                Row = 12312412,
                Number = 1234124,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(seat);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public async Task CreateSeat__ShouldReturnAddedSeat()
        {
            // Arrange
            SeatDto seat = new ()
            {
                AreaId = 1,
                Row = 12435212,
                Number = 123123,
            };

            // Act
            var addedSeat = await _service.CreateAsync(seat);
            seat.Id = addedSeat.Id;

            // Assert
            seat.Should().BeEquivalentTo(addedSeat);

            // Delete added seat
            await _service.DeleteAsync(addedSeat);
        }

        [Test]
        public void UpdateSeat_WhenAreaIdDoesntExist_ShouldReturnValidationException()
        {
            // Arrange
            SeatDto seat = new ()
            {
                Id = 1,
                AreaId = 0,
                Row = 12312412,
                Number = 1234124,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(seat);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public async Task UpdateSeat_ShouldReturnUpdatedSeat()
        {
            // Arrange
            SeatDto seat = new ()
            {
                Id = 1,
                AreaId = 1,
                Row = 123123,
                Number = 1,
            };

            // Act
            var updatedSeat = await _service.UpdateAsync(seat);

            // Assert
            seat.Should().BeEquivalentTo(updatedSeat);

            // Back to old
            updatedSeat.Row = 1;
            await _service.UpdateAsync(updatedSeat);
        }

        [Test]
        public async Task GetSeatById_ShouldReturnFoundSeat()
        {
            // Arrange
            int id = 1;

            // Act
            var foundSeat = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundSeat.Id);
        }
    }
}
