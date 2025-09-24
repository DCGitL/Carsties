using System;
using HealthService.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthService;

public class HealthStatusService : IHealthStatusService
{
    private readonly HealthCheckService _healthCheckService;

    public HealthStatusService(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    public async Task<HealthCheckResponse> GetHealthStatusAsync(string? tag)
    {

        var report = await _healthCheckService.CheckHealthAsync(x => string.IsNullOrEmpty(tag) || x.Tags.Contains(tag));


        var response = new HealthCheckResponse
        {
            Status = report.Status.ToString(),
            Checks = report.Entries.Select(entry => new HealthCheck
            {
                Tags = entry.Value.Tags,
                Component = entry.Key,
                Status = entry.Value.Status.ToString(),
                Description = entry.Value.Description ?? string.Empty,
                ExceptionMessage = entry.Value.Exception?.Message ?? string.Empty,
                Duration = entry.Value.Duration,
                Data = entry.Value.Data
            }),
            Duration = report.TotalDuration
        };

        return response;
    }
}
