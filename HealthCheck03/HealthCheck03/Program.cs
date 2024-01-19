using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using HealthCheck03.HealthChecks;

namespace HealthCheck03
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks()
                .AddCheck<DBCheck>("DB check");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Use custom middleware to return JSON response for health checks
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = ResponseWriter.WriteResponse
            });

            app.MapControllers();

            app.Run();
        }

        
    }
}
