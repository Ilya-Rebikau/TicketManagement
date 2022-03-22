using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TicketManagement.Web;
using Xunit;

namespace TicketManagement.ControllersIntegrationTests
{
    public class TicketsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public TicketsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/Tickets/Index")]
        [InlineData("/Tickets/Create")]
        [InlineData("/Tickets/Edit")]
        [InlineData("/Tickets/Delete")]
        [InlineData("/Tickets/Details")]
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
