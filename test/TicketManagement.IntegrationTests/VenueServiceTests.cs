using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.IntegrationTests
{
    [TestFixture]
    internal class VenueServiceTests
    {
        private IService<Venue> _service;

        [SetUp]
        public void Setup()
        {
            var venueRepository = new VenueRepository();
            _service = new VenueService(venueRepository);
        }

        [Test]
        public void CreateVenue_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Venue venue = new ()
            {
                Description = null,
                Address = "Address",
                Phone = null,
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(venue);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void CreateVenue_WhenAddressIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Venue venue = new ()
            {
                Description = "Description",
                Address = null,
                Phone = null,
                Name = "Name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(venue);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void CreateVenue_WhenNameIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Venue venue = new ()
            {
                Description = "Description",
                Address = "Address",
                Phone = null,
                Name = null,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(venue);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void CreateVenue_WhenNameIsntUnique_ShouldReturnSqlException()
        {
            // Arrange
            Venue venue = new ()
            {
                Description = "Description",
                Address = "Address",
                Phone = null,
                Name = "First venue name",
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.CreateAsync(venue);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task CreateVenue__ShouldReturnAddedVenue()
        {
            // Arrange
            Venue venue = new ()
            {
                Description = "Description",
                Address = "Address",
                Phone = "Phone",
                Name = "Name",
            };

            // Act
            Venue addedVenue = await _service.CreateAsync(venue);

            // Assert
            Assert.AreEqual(venue, addedVenue);

            // DeleteAsync added venue
            var venues = await _service.GetAllAsync();
            await _service.DeleteAsync(venues.Last());
        }

        [Test]
        public void UpdateVenue_WhenDescriptionIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Venue venue = new ()
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
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void UpdateVenue_WhenAddressIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Venue venue = new ()
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
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public void UpdateVenue_WhenNameIsNull_ShouldReturnSqlException()
        {
            // Arrange
            Venue venue = new ()
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
            Assert.ThrowsAsync<SqlException>(testAction);
        }

        [Test]
        public async Task UpdateVenue_WhenVenueDoesntExist_ShouldReturnEmptyVenue()
        {
            // Arrange
            Venue venue = new ()
            {
                Id = 0,
                Description = "Description",
                Address = "Address",
                Phone = "Phone",
                Name = "Name",
            };

            // Act
            Venue updatedVenue = await _service.UpdateAsync(venue);

            // Assert
            Assert.AreEqual(0, updatedVenue.Id);
        }

        [Test]
        public async Task UpdateVenue_ShouldReturnUpdatedVenue()
        {
            // Arrange
            Venue venue = new ()
            {
                Id = 1,
                Description = "First venue description",
                Address = "First venue address",
                Phone = "New phone",
                Name = "First venue name",
            };

            // Act
            Venue updatedVenue = await _service.UpdateAsync(venue);

            // Assert
            Assert.AreEqual(venue, updatedVenue);

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
            Venue foundVenue = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, foundVenue.Id);
        }

        [Test]
        public async Task GetVenueById_WhenVenueDoesntExist_ShouldReturnEmptyVenue()
        {
            // Arrange
            int id = -1;

            // Act
            Venue venue = await _service.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(0, venue.Id);
        }

        [Test]
        public void DeleteVenue_WhenThereAreLayoutsInIt_ShouldReturnSqlException()
        {
            // Arrange
            Venue venue = new ()
            {
                Id = 1,
            };

            // Act
            AsyncTestDelegate testAction = async () => await _service.DeleteAsync(venue);

            // Assert
            Assert.ThrowsAsync<SqlException>(testAction);
        }
    }
}
