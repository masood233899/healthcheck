using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck03
{
    public class DBCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = new SqlConnection("data source= KANINI-LTP-682; initial catalog =VAN; integrated security=SSPI;TrustServerCertificate=True;");
                await connection.OpenAsync(cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("DB connection issue", ex, new Dictionary<string, object> { { "ExceptionDetails", ex.ToString() } });
            }
        }
    }

}
