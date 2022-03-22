using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TicketManagement.DataAccess;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.RepositoriesEf;
using TicketManagement.EventManagerAPI.Automapper;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;
using TicketManagement.EventManagerAPI.Services;

namespace TicketManagement.IntegrationTests
{
    internal class EventSeatEfServiceTests
    {
        private IService<EventSeatDto> _service;

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
            var eventSeatRepository = new EfRepository<EventSeat>(context);
            var converter = new ModelsConverter<EventSeat, EventSeatDto>(mapper);
            _service = new EventSeatService(eventSeatRepository, converter);
        }

        [Test]
        public void CreateEventSeat_WhenEventAreaIdDoesntExist_ShouldReturnDbUpdateException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                EventAreaId = 0,
                Row = 12312412,
                Number = 1234124,
                State = PlaceStatus.Free,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public async Task CreateEventSeat__ShouldReturnAddedEventSeat()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                EventAreaId = 1,
                Row = 12435212,
                Number = 123123,
                State = PlaceStatus.Free,
            };

            // Act
            var addedEventSeat = await _service.CreateAsync(eventSeat);
            eventSeat.Id = addedEventSeat.Id;

            // Assert
            eventSeat.Should().BeEquivalentTo(addedEventSeat);

            // Delete added event seat
            await _service.DeleteAsync(addedEventSeat);
        }

        [Test]
        public void UpdateEventSeat_WhenEventAreaIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Id = 1,
                EventAreaId = 0,
                Row = 12312412,
                Number = 1234124,
                State = PlaceStatus.Free,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventSeat);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public async Task UpdateEventSeat_ShouldReturnUpdatedEventSeat()
        {
            // Arrange
            EventSeatDto eventSeat = new ()
            {
                Id = 1,
                EventAreaId = 1,
                Row = 21234142,
                Number = 1,
                State = PlaceStatus.Free,
            };

            // Act
            var updatedEventSeat = await _service.UpdateAsync(eventSeat);

            // Assert
            eventSeat.Should().BeEquivalentTo(updatedEventSeat);

            // Back to old
            updatedEventSeat.Row = 1;
            await _service.UpdateAsync(updatedEventSeat);
        }

        [Test]
        public async Task GetEventSeatById_ShouldReturnFoundEventSeat()
        {
            // Arrange
            int id = 1;

            // Act
            var foundEventSeat = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundEventSeat.Id);
        }
    }
}
