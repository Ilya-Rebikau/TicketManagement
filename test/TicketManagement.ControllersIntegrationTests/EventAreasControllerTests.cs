using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TicketManagement.Web;
using Xunit;

namespace TicketManagement.ControllersIntegrationTests
{
    public class EventAreasControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public EventAreasControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/EventAreas/Index")]
        [InlineData("/EventAreas/Create")]
        [InlineData("/EventAreas/Edit")]
        [InlineData("/EventAreas/Delete")]
        [InlineData("/EventAreas/Details")]
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
