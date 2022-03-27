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
using TicketManagement.VenueManagerAPI.Automapper;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;
using TicketManagement.VenueManagerAPI.Services;

namespace TicketManagement.IntegrationTests
{
    [TestFixture]
    internal class VenueEfServiceTests
    {
        private IService<VenueDto> _service;
        private IService<LayoutDto> _layoutService;

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
            var layoutRepository = new LayoutEfRepository(context);
            var eventRepository = new EventEfRepository(context);
            var eventAreaRepository = new EventAreaEfRepository(context);
            var eventSeatRepository = new EfRepository<EventSeat>(context);
            var venueRepository = new VenueEfRepository(context);
            var venueConverter = new ModelsConverter<Venue, VenueDto>(mapper);
            var layoutConverter = new ModelsConverter<Layout, LayoutDto>(mapper);
            _service = new VenueService(venueRepository, venueConverter, layoutRepository, eventRepository,
                eventAreaRepository, eventSeatRepository, configuration);
            _layoutService = new LayoutService(layoutRepository, layoutConverter, eventRepository, eventAreaRepository,
                eventSeatRepository, configuration);
        }

        [Test]
        public void CreateVenue_WhenDescriptionIsNull_ShouldReturnDbUpdateException()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Description = null,
                Address = "Address",
                Phone = null,
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(venue);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public void CreateVenue_WhenAddressIsNull_ShouldReturnDbUpdateException()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Description = "Description",
                Address = null,
                Phone = null,
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(venue);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public void CreateVenue_WhenNameIsNull_ShouldReturnDbUpdateException()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Description = "Description",
                Address = "Address",
                Phone = null,
                Name = null,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(venue);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public void CreateVenue_WhenNameIsntUnique_ShouldReturnDbUpdateException()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Description = "Description",
                Address = "Address",
                Phone = null,
                Name = "First venue name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(venue);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public async Task CreateVenue__ShouldReturnAddedVenue()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Description = "Description",
                Address = "Address",
                Phone = "Phone",
                Name = "Name",
            };

            // Act
            var addedVenue = await _service.CreateAsync(venue);
            venue.Id = addedVenue.Id;

            // Assert
            venue.Should().BeEquivalentTo(addedVenue);

            // DeleteAsync added venue
            await _service.DeleteAsync(addedVenue);
        }

        [Test]
        public void UpdateVenue_WhenDescriptionIsNull_ShouldReturnDbUpdateException()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Id = 1,
                Description = null,
                Address = "Address",
                Phone = null,
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(venue);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public void UpdateVenue_WhenAddressIsNull_ShouldReturnDbUpdateException()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Id = 1,
                Description = "Description",
                Address = null,
                Phone = null,
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(venue);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public void UpdateVenue_WhenNameIsNull_ShouldReturnDbUpdateException()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Id = 1,
                Description = "Description",
                Address = "Address",
                Phone = null,
                Name = null,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.UpdateAsync(venue);

            // Assert
            Assert.ThrowsAsync<DbUpdateException>(testAction);
        }

        [Test]
        public async Task UpdateVenue_ShouldReturnUpdatedVenue()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Id = 1,
                Description = "First venue description",
                Address = "First venue address",
                Phone = "New phone",
                Name = "First venue name",
            };

            // Act
            var updatedVenue = await _service.UpdateAsync(venue);

            // Assert
            venue.Should().BeEquivalentTo(updatedVenue);

            // Back to old
            updatedVenue.Description = "123 45 678 90 12";
            await _service.UpdateAsync(updatedVenue);
        }

        [Test]
        public async Task GetVenueById_ShouldReturnFoundVenue()
        {
            // Arrange
            int id = 1;

            // Act
            var foundVenue = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundVenue.Id);
        }

        [Test]
        public async Task DeleteVenue_WhenThereAreItemsInIt_ShouldDeleteAllInIt()
        {
            // Arrange
            VenueDto venue = new ()
            {
                Name = "NewName",
                Address = "Address",
                Description = "Description",
            };
            var addedVenue = await _service.CreateAsync(venue);
            var allLayouts = await _layoutService.GetAllAsync(1);
            int layoutsCount = allLayouts.Count();
            LayoutDto layout = new ()
            {
                VenueId = addedVenue.Id,
                Description = "Description",
                Name = "Name",
            };
            await _layoutService.CreateAsync(layout);

            // Act
            await _service.DeleteAsync(addedVenue);
            var allNewLayouts = await _layoutService.GetAllAsync(1);
            var newLayoutsCount = allNewLayouts.Count();

            // Assert
            Assert.AreEqual(layoutsCount, newLayoutsCount);
        }
    }
}
