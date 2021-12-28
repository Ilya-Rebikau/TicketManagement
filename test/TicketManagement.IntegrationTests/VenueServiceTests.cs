using System;
using System.Data.SqlClient;
using System.Linq;
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
            TestDelegate testAction = () => _service.Create(venue);

            // Assert
            Assert.Throws<SqlException>(testAction);
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
            TestDelegate testAction = () => _service.Create(venue);

            // Assert
            Assert.Throws<SqlException>(testAction);
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
            TestDelegate testAction = () => _service.Create(venue);

            // Assert
            Assert.Throws<SqlException>(testAction);
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
            TestDelegate testAction = () => _service.Create(venue);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void CreateVenue__ShouldReturnAddedVenue()
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
            Venue addedVenue = _service.Create(venue);

            // Assert
            Assert.AreEqual(venue, addedVenue);

            // Delete added venue
            _service.Delete(_service.GetAll().Last());
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
            TestDelegate testAction = () => _service.Update(venue);

            // Assert
            Assert.Throws<SqlException>(testAction);
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
            TestDelegate testAction = () => _service.Update(venue);

            // Assert
            Assert.Throws<SqlException>(testAction);
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
            TestDelegate testAction = () => _service.Update(venue);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }

        [Test]
        public void UpdateVenue_WhenVenueDoesntExist_ShouldReturnEmptyVenue()
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
            Venue updatedVenue = _service.Update(venue);

            // Assert
            Assert.AreEqual(0, updatedVenue.Id);
        }

        [Test]
        public void UpdateVenue_ShouldReturnUpdatedVenue()
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
            Venue updatedVenue = _service.Update(venue);

            // Assert
            Assert.AreEqual(venue, updatedVenue);

            // Back to old
            updatedVenue.Description = "123 45 678 90 12";
            _service.Update(updatedVenue);
        }

        [Test]
        public void GetVenueById_ShouldReturnFoundVenue()
        {
            // Arrange
            int id = 1;

            // Act
            Venue foundVenue = _service.GetById(id);

            // Assert
            Assert.AreEqual(id, foundVenue.Id);
        }

        [Test]
        public void GetVenueById_WhenVenueDoesntExist_ShouldReturnEmptyVenue()
        {
            // Arrange
            int id = -1;

            // Act
            Venue venue = _service.GetById(id);

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
            TestDelegate testAction = () => _service.Delete(venue);

            // Assert
            Assert.Throws<SqlException>(testAction);
        }
    }
}
