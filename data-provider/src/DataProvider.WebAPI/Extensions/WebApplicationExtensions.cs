using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;
using System.Text.Json;

namespace DataProvider.WebAPI.Extensions
{
    /// <summary>
    /// WebApplication extension methods
    /// </summary>
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Extension method for configure a custom health check
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns>Application builder itself</returns>
        public static IApplicationBuilder UseCustomtHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthz", new HealthCheckOptions());

            app.UseHealthChecks("/healthz/detailed", new HealthCheckOptions
            {
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                },
                ResponseWriter = WriteResponse
            });

            return app;
        }

        /// <summary>
        /// Write a custom health check response
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="healthReport">Health check report</param>
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

                    jsonWriter.WriteString("status", healthReportEntry.Value.Status.ToString());

                    jsonWriter.WriteString("description", healthReportEntry.Value.Description);

                    jsonWriter.WriteStartObject("data");

                    foreach (var item in healthReportEntry.Value.Data)
                    {
                        jsonWriter.WritePropertyName(item.Key);

                        JsonSerializer.Serialize(jsonWriter, item.Value, item.Value?.GetType() ?? typeof(object));
                    }

                    jsonWriter.WriteEndObject();

                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndObject();

                jsonWriter.WriteEndObject();
            }

            return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }
    }
}
