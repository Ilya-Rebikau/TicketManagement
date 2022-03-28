using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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
using TicketManagement.VenueManagerAPI.ModelsDTO;
using TicketManagement.VenueManagerAPI.Services;

namespace TicketManagement.IntegrationTests
{
    [TestFixture]
    internal class EventEfServiceTests
    {
        private IService<EventDto> _service;
        private IService<EventAreaDto> _eventAreaService;
        private VenueManagerAPI.Interfaces.IService<AreaDto> _areaService;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<TicketManagementContext>();

            builder.UseSqlServer(DbConnection.GetStringConnection())
                    .UseInternalServiceProvider(serviceProvider);
            var configVenue = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new VenueManagerAPI.Automapper.AutoMapperProfile());
            });
            var mapperVenue = configVenue.CreateMapper();
            var configEvent = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapperEvent = configEvent.CreateMapper();
            var configuration = new ConfigurationManager();
            configuration.AddJsonFile("appsettings.json");
            var context = new TicketManagementContext(builder.Options);
            var eventRepository = new EventEfRepository(context);
            var converter = new ModelsConverter<Event, EventDto>(mapperEvent);
            var eventAreaRepository = new EventAreaEfRepository(context);
            var eventSeatRepository = new EfRepository<EventSeat>(context);
            var areaRepository = new AreaEfRepository(context);
            _areaService = new AreaService(areaRepository, new VenueManagerAPI.Automapper.ModelsConverter<Area, AreaDto>(mapperVenue), configuration);
            _eventAreaService = new EventAreaService(eventAreaRepository, new ModelsConverter<EventArea, EventAreaDto>(mapperEvent), eventSeatRepository, configuration);
            _service = new EventCrudService(eventRepository, converter, eventAreaRepository, eventSeatRepository, configuration);
        }

        [Test]
        public void CreateEvent_WhenNameIsNull_ShouldReturnSqlException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Name = null,
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
                ImageUrl = "image",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Name = "Name",
                Description = null,
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
                ImageUrl = "image",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void CreateEvent_WhenLayoudIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Name = "Name",
                Description = "Description",
                LayoutId = 0,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task CreateEvent__ShouldReturnAddedEvent()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Name = "Name",
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
                ImageUrl = "image",
            };

            // Act
            var addedEvent = await _service.CreateAsync(eventModel);
            eventModel.Id = addedEvent.Id;

            // Assert
            eventModel.Should().BeEquivalentTo(addedEvent);

            // Delete added event
            await _service.DeleteAsync(addedEvent);
        }

        [Test]
        public void UpdateEvent_WhenNameIsNull_ShouldReturnSqlException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Id = 1,
                Name = null,
                Description = "Description",
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
                ImageUrl = "image",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Id = 1,
                Name = "Name",
                Description = null,
                LayoutId = 1,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
                ImageUrl = "image",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void UpdateEvent_WhenLayoudIdDoesntExist_ShouldReturnSqlException()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                LayoutId = 0,
                TimeStart = new DateTime(2050, 12, 21, 15, 10, 0),
                TimeEnd = new DateTime(2050, 12, 21, 16, 40, 0),
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(eventModel);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task UpdateEvent_ShouldReturnUpdatedEvent()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Id = 1,
                Name = "New name",
                Description = "First event description",
                LayoutId = 1,
                TimeStart = new DateTime(2030, 12, 21, 15, 0, 0),
                TimeEnd = new DateTime(2030, 12, 21, 17, 0, 0),
                ImageUrl = "Url",
            };

            // Act
            var updatedEvent = await _service.UpdateAsync(eventModel);

            // Assert
            eventModel.Should().BeEquivalentTo(updatedEvent);

            // Back to old
            updatedEvent.Name = "First event name";
            await _service.UpdateAsync(updatedEvent);
        }

        [Test]
        public async Task GetEventById_ShouldReturnFoundEvent()
        {
            // Arrange
            int id = 1;

            // Act
            var foundEvent = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundEvent.Id);
        }

        [Test]
        public async Task DeleteEvent_WhenThereAreEventAreasInIt_ShouldDeleteEventAreasAndEventSeatsInIt()
        {
            // Arrange
            EventDto eventModel = new ()
            {
                Description = "Descr",
                ImageUrl = "Url",
                LayoutId = 1,
                Name = "Name",
                TimeStart = new DateTime(2050, 1, 1),
                TimeEnd = new DateTime(2051, 1, 1),
            };

            var areasInLayout = await _areaService.GetAllAsync(1);
            var areasCount = areasInLayout.Count(a => a.LayoutId == eventModel.LayoutId);
            var addedEvent = await _service.CreateAsync(eventModel);
            var eventAreas = await _eventAreaService.GetAllAsync(1);
            int eventAreasCount = eventAreas.Count(e => e.EventId == addedEvent.Id);
            EventAreaDto eventArea = new ()
            {
                CoordX = 12451,
                CoordY = 123123,
                Description = "Description.",
                EventId = 1,
                Price = 1,
            };
            await _eventAreaService.CreateAsync(eventArea);

            // Act
            await _service.DeleteAsync(addedEvent);
            var newEventAreas = await _eventAreaService.GetAllAsync(1);
            int newEventAreasCount = newEventAreas.Count(e => e.Id == addedEvent.Id);

            // Assert
            Assert.AreEqual(eventAreasCount, newEventAreasCount + areasCount);
        }
    }
}
