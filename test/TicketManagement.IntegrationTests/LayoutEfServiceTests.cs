using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using RestEase;
using TicketManagement.DataAccess;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.RepositoriesEf;
using TicketManagement.EventManagerAPI.ModelsDTO;
using TicketManagement.EventManagerAPI.Services;
using TicketManagement.VenueManagerAPI.Automapper;
using TicketManagement.VenueManagerAPI.Infrastructure;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;
using TicketManagement.VenueManagerAPI.Services;
using TicketManagement.Web.Interfaces.HttpClients;

namespace TicketManagement.IntegrationTests
{
    [TestFixture]
    internal class LayoutEfServiceTests
    {
        private EventManagerAPI.Interfaces.IService<EventDto> _eventService;
        private IService<LayoutDto> _service;

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
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapperVenue = configVenue.CreateMapper();
            var configEvent = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EventManagerAPI.Automapper.AutoMapperProfile());
            });
            var mapperEvent = configEvent.CreateMapper();
            var configuration = new ConfigurationManager();
            configuration.AddJsonFile("appsettings.json");
            var context = new TicketManagementContext(builder.Options);
            var layoutRepository = new LayoutEfRepository(context);
            var eventRepository = new EventEfRepository(context);
            var eventAreaRepository = new EventAreaEfRepository(context);
            var eventSeatRepository = new EfRepository<EventSeat>(context);
            var converter = new ModelsConverter<Layout, LayoutDto>(mapperVenue);
            _service = new LayoutService(layoutRepository, converter, eventRepository, eventAreaRepository, eventSeatRepository, configuration);
            _eventService = new EventCrudService(eventRepository,
                new EventManagerAPI.Automapper.ModelsConverter<Event, EventDto>(mapperEvent), eventAreaRepository, eventSeatRepository, configuration);
        }

        [Test]
        public void CreateLayout_WhenVenueIdDoesntExist_ShouldReturnValidationException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                VenueId = 0,
                Description = "Description",
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateLayout_WhenDescriptionIsNull_ShouldReturnValidationException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                VenueId = 1,
                Description = null,
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void CreateLayout_WhenNameIsNull_ShouldReturnValidationException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = null,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public async Task CreateLayout__ShouldReturnAddedLayout()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = "Name",
            };

            // Act
            var addedLayout = await _service.CreateAsync(layout);
            layout.Id = addedLayout.Id;

            // Assert
            layout.Should().BeEquivalentTo(addedLayout);

            // DeleteAsync added layout
            await _service.DeleteAsync(addedLayout);
        }

        [Test]
        public void UpdateLayout_WhenVenueIdDoesntExist_ShouldReturnValidationException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                Id = 1,
                VenueId = 0,
                Description = "Description",
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenDescriptionIsNull_ShouldReturnValidationException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                Id = 1,
                VenueId = 1,
                Description = null,
                Name = "NewName",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenNameIsNull_ShouldReturnValidationException()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                Id = 1,
                VenueId = 1,
                Description = "Description",
                Name = null,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(layout);

            // Assert
            Assert.ThrowsAsync<ValidationException>(testAction);
        }

        [Test]
        public async Task UpdateLayout_ShouldReturnUpdatedLayout()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                Id = 1,
                VenueId = 1,
                Description = "New description",
                Name = "First layout",
            };

            // Act
            var updatedLayout = await _service.UpdateAsync(layout);

            // Assert
            layout.Should().BeEquivalentTo(updatedLayout);

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
            var foundLayout = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundLayout.Id);
        }

        [Test]
        public async Task DeleteLayout_WhenThereAreAreasInIt_ShouldDeleteAllInIt()
        {
            // Arrange
            LayoutDto layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = "Name",
            };
            var addedLayout = await _service.CreateAsync(layout);
            var events = await _eventService.GetAllAsync(1);
            int eventsCount = events.Count();
            EventDto @event = new ()
            {
                LayoutId = addedLayout.Id,
                Description = "Description",
                ImageUrl = "Url",
                Name = "Name",
                TimeStart = new DateTime(2057, 1, 1),
                TimeEnd = new DateTime(2058, 1, 1),
            };
            await _eventService.CreateAsync(@event);

            // Act
            await _service.DeleteAsync(addedLayout);
            var newEvents = await _eventService.GetAllAsync(1);
            int newEventsCount = newEvents.Count();

            // Assert
            Assert.AreEqual(eventsCount, newEventsCount);
        }
    }
}
