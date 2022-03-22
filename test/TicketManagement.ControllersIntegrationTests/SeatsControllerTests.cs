using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TicketManagement.Web;
using Xunit;

namespace TicketManagement.ControllersIntegrationTests
{
    public class SeatsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public SeatsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/Seats/Index")]
        [InlineData("/Seats/Create")]
        [InlineData("/Seats/Edit")]
        [InlineData("/Seats/Delete")]
        [InlineData("/Seats/Details")]
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