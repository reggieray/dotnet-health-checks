using System.Net;

namespace Healthz.Api.Tests
{
    public class HealthzEndpointTests
    {
        [Theory]
        [InlineData("/healthz/all")]
        [InlineData("/healthz/ready")]
        [InlineData("/healthz/live")]
        public async Task GetHealthz_ShouldReturnHealthChecks(string path)
        {
            using var application = new HealthzApiApplication();
            var httpClient = application.CreateClient();

            var response = await httpClient.GetAsync(path).ConfigureAwait(false);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
