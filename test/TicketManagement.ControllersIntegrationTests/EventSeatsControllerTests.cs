using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TicketManagement.Web;
using Xunit;

namespace TicketManagement.ControllersIntegrationTests
{
    public class EventSeatsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public EventSeatsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/EventSeats/Index")]
        [InlineData("/EventSeats/Create")]
        [InlineData("/EventSeats/Edit")]
        [InlineData("/EventSeats/Delete")]
        [InlineData("/EventSeats/Details")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}