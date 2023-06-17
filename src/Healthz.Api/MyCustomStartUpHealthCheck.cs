using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Healthz.Api
{
    public class MyCustomStartUpHealthCheck : IHealthCheck
    {
        private readonly IConfiguration configuration;

        public MyCustomStartUpHealthCheck(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            if (bool.TryParse(this.configuration[$"HealthCheck:{nameof(MyCustomStartUpHealthCheck)}"], out var result) && result)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }

            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}
