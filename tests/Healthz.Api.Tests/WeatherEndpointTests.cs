namespace Healthz.Api.Tests
{
    public class WeatherEndpointTests
    {
        [Fact]
        public async Task GetWeatherForecast_ShouldReturnWeatherForecast()
        {
            using var application = new HealthzApiApplication();
            var httpClient = application.CreateClient();

            var response = await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("/weatherforecast/").ConfigureAwait(false);

            response.Should().NotBeNull();
            response!.Count().Should().Be(5);
        }
    }
}