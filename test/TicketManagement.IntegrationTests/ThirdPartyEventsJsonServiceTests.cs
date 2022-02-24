using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.RepositoriesEf;
using TicketManagement.DataAccess.RepositoriesJson;

namespace TicketManagement.IntegrationTests
{
    [TestFixture]
    public class ThirdPartyEventsJsonServiceTests
    {
        private IReaderService<EventDto> _reader;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<TicketManagementContext>();

            builder.UseSqlServer(DbConnection.GetStringConnection())
                    .UseInternalServiceProvider(serviceProvider);

            var converter = new BaseConverter<ThirdPartyEvent, EventDto>();
            var thirdPartyEventRepository = new ThirdPartyEventRepositoryJson();
            _reader = new BaseReaderService<ThirdPartyEvent, EventDto>(thirdPartyEventRepository, converter);
        }

        [Test]
        public async Task GetEvents_ShouldReturnFoundEvents()
        {
            // Arrange
            var name = "xxx";
            int count = 1;
            var json = "{\"events\":[{\"Id\":1,\"Name\":\"xxx\",\"StartDate\":\"2022-05-05T19:03:00Z\",\"EndDate\":\"2022-06-30T19:03:00Z\",\"Description\":\"xxx\",\"PosterImage\":\"Image\"," +
                "\"LayoutId\": 1}]}";

            // Act
            var foundEvents = await _reader.GetAllAsync(json);
            var @event = foundEvents.SingleOrDefault(ev => ev.Name == name);

            // Assert
            Assert.IsTrue(name == @event.Name && count == foundEvents.Count());
        }

        [Test]
        public async Task GetEventById_ShouldReturnFoundEvent()
        {
            // Arrange
            int id = 1;
            var json = "{\"events\":[{\"Id\":1,\"Name\":\"xxx\",\"StartDate\":\"2022-05-05T19:03:00Z\",\"EndDate\":\"2022-06-30T19:03:00Z\",\"Description\":\"xxx\",\"PosterImage\":\"Image\"," +
                "\"LayoutId\": 1}]}";

            // Act
            var foundEvent = await _reader.GetByIdAsync(id, json);

            // Assert
            Assert.AreEqual(id, foundEvent.Id);
        }
    }
}
