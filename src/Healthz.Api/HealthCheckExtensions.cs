using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text;

namespace Healthz.Api
{
    public static class HealthCheckExtensions
    {
        public static void RegisterHealthChecks(this IServiceCollection services, string? healthCheckUri)
        {
            var url = string.IsNullOrEmpty(healthCheckUri) ? "http://localhost" : healthCheckUri;

            services.AddHealthChecks()
                .AddUrlGroup(new Uri(url), name: "UriCheck", tags: new[] { "live", "all" })
                .AddCheck<MyCustomStartUpHealthCheck>(nameof(MyCustomStartUpHealthCheck), tags: new[] { "ready", "all" });
        }

        public static void MapHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/healthz/live", new HealthCheckOptions
            {
                ResponseWriter = WriteResponse,
                Predicate = healthCheck => healthCheck.Tags.Contains("live")
            });

            app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
            {
                ResponseWriter = WriteResponse,
                Predicate = healthCheck => healthCheck.Tags.Contains("ready")
            });

            app.MapHealthChecks("/healthz/all", new HealthCheckOptions
            {
                ResponseWriter = WriteResponse,
                Predicate = healthCheck => healthCheck.Tags.Contains("all")
            });
        }

        private static Task WriteResponse(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions { Indented = true };

            using var memoryStream = new MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("status", healthReport.Status.ToString());
                jsonWriter.WriteStartObject("results");

                foreach (var healthReportEntry in healthReport.Entries)
                {
                    jsonWriter.WriteStartObject(healthReportEntry.Key);
                    jsonWriter.WriteString("status",
                        healthReportEntry.Value.Status.ToString());
                    jsonWriter.WriteString("description",
                        healthReportEntry.Value.Description);
                    jsonWriter.WriteStartObject("data");

                    foreach (var item in healthReportEntry.Value.Data)
                    {
                        jsonWriter.WritePropertyName(item.Key);

                        JsonSerializer.Serialize(jsonWriter, item.Value,
                            item.Value?.GetType() ?? typeof(object));
                    }

                    jsonWriter.WriteEndObject();
                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            return context.Response.WriteAsync(
                Encoding.UTF8.GetString(memoryStream.ToArray()));
        }
    }
}
