using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TicketManagement.Web;
using Xunit;

namespace TicketManagement.ControllersIntegrationTests
{
    public class AreasControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AreasControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Areas/Index")]
        [InlineData("/Areas/Create")]
        [InlineData("/Areas/Edit")]
        [InlineData("/Areas/Details")]
        [InlineData("/Areas/Delete")]
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