using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace HealthCheck03.HealthChecks
{
    public class ResponseWriter
    {
        public static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var StatusCode = 0;


            if (result.Status.ToString() == "Healthy") 
            {
                StatusCode = StatusCodes.Status200OK;
            }
            else if (result.Status.ToString() == "Unhealthy")
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable;
            }
            else if (result.Status.ToString() == "Degraded")
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }

            var response = new
            {
                Status_Code = StatusCode,
                Status = result.Status.ToString(),
                TotalDuration = result.TotalDuration,
                Entries = result.Entries.Select(e => new
                {
                    Key = e.Key,
                    Value = e.Value.Status.ToString(),
                    Description = e.Value.Description,
                    Duration = e.Value.Duration,
                    ExceptionDetails = e.Value.Data.TryGetValue("ExceptionDetails", out var exDetails) ? exDetails?.ToString() : null
                })
            };

            return httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
