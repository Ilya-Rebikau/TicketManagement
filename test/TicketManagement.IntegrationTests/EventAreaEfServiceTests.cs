using System.Linq;
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
using TicketManagement.EventManagerAPI.Automapper;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;
using TicketManagement.EventManagerAPI.Services;

namespace TicketManagement.IntegrationTests
{
    [TestFixture]
    internal class EventAreaServiceTests
    {
        private IService<EventAreaDto> _service;
        private IService<EventSeatDto> _eventSeatService;

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
            var eventAreaRepository = new EventAreaEfRepository(context);
            var converter = new ModelsConverter<EventArea, EventAreaDto>(mapper);
            var eventSeatRepository = new EfRepository<EventSeat>(context);
            _eventSeatService = new EventSeatService(eventSeatRepository, new ModelsConverter<EventSeat, EventSeatDto>(mapper), configuration);
            _service = new EventAreaService(eventAreaRepository, converter, eventSeatRepository, configuration);
        }

        [Test]
        public void CreateEventArea_WhenEventIdDoesntExist_ShouldReturnDbUpdateException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                EventId = 0,
                Description = "Description",
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public void CreateEventArea_WhenDescriptionIsNull_ShouldReturnDbUpdateException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                EventId = 1,
                Description = null,
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public async Task CreateArea__ShouldReturnAddedArea()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                EventId = 1,
                Description = "Description",
                CoordX = 33123123,
                CoordY = 134123,
                Price = 1,
            };

            // Act
            var addedEventArea = await _service.CreateAsync(eventArea);
            eventArea.Id = addedEventArea.Id;

            // Assert
            eventArea.Should().BeEquivalentTo(addedEventArea);

            // DeleteAsync added event area
            await _service.DeleteAsync(addedEventArea);
        }

        [Test]
        public void UpdateEventArea_WhenLayoutIdDoesntExist_ShouldReturnDbUpdateException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Id = 1,
                EventId = 0,
                Description = "Description",
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public void UpdateEventArea_WhenDescriptionIsNull_ShouldReturnDbUpdateException()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Id = 1,
                EventId = 1,
                Description = null,
                CoordX = 123123123,
                CoordY = 5325235,
                Price = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventArea);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public async Task UpdateEventArea_ShouldReturnUpdatedArea()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                Id = 1,
                EventId = 1,
                Description = "First event area description",
                CoordX = 1,
                CoordY = 1,
                Price = 25,
            };

            // Act
            var updatedEventArea = await _service.UpdateAsync(eventArea);

            // Assert
            eventArea.Should().BeEquivalentTo(updatedEventArea);

            // Back to old
            updatedEventArea.Price = 11;
            await _service.UpdateAsync(updatedEventArea);
        }

        [Test]
        public async Task GetAreaById_ShouldReturnFoundArea()
        {
            // Arrange
            int id = 1;

            // Act
            var foundEventArea = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundEventArea.Id);
        }

        [Test]
        public async Task DeleteEventArea_WhenThereAreEventSeatsInIt_ShouldDeleteEventSeats()
        {
            // Arrange
            EventAreaDto eventArea = new ()
            {
                CoordX = 1124124,
                CoordY = 1123123,
                Description = "Desc",
                EventId = 1,
                Price = 11,
            };
            var addedEventArea = await _service.CreateAsync(eventArea);
            var eventSeats = await _eventSeatService.GetAllAsync(1);
            int eventsSeatsCount = eventSeats.Count();
            EventSeatDto eventSeat = new ()
            {
                EventAreaId = addedEventArea.Id,
                Number = 1,
                Row = 1,
                State = PlaceStatus.Free,
            };
            await _eventSeatService.CreateAsync(eventSeat);

            // Act
            await _service.DeleteAsync(addedEventArea);
            var newEventSeats = await _eventSeatService.GetAllAsync(1);
            int newEventSeatsCount = newEventSeats.Count();

            // Assert
            Assert.AreEqual(eventsSeatsCount, newEventSeatsCount);
        }
    }
}
