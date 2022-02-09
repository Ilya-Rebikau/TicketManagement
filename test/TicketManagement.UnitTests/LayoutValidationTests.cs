﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.UnitTests
{
    internal class LayoutValidationTests
    {
        private IService<Layout> _service;

        [SetUp]
        public void Setup()
        {
            var layoutRepositoryMock = new Mock<IRepository<Layout>>();
            layoutRepositoryMock.Setup(rep => rep.GetAll()).Returns(GetTestLayouts());
            _service = new LayoutService(layoutRepositoryMock.Object);
        }

        private IEnumerable<Layout> GetTestLayouts()
        {
            IEnumerable<Layout> layouts = new List<Layout>
            {
                new Layout { Id = 1, VenueId = 1, Name = "First layout", Description = "First layout description" },
                new Layout { Id = 2, VenueId = 1, Name = "Second layout", Description = "Second layout description" },
            };
            return layouts;
        }

        [Test]
        public void CreateLayout_WhenNameIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Layout layout = new ()
            {
                VenueId = 1,
                Description = "Description",
                Name = "First layout",
            };

            // Act
            TestDelegate testAction = () => _service.Create(layout);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Test]
        public void UpdateLayout_WhenNameIsntUnique_ShouldReturnArgumentException()
        {
            // Arrange
            Layout layout = new ()
            {
                Id = 2,
                VenueId = 1,
                Description = "Description",
                Name = "First layout",
            };

            // Act
            TestDelegate testAction = () => _service.Update(layout);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }
}