using System;

namespace HealthService.Models;

public class HealthCheckResponse
{
    public string Status { get; set; } = default!;
    public IEnumerable<HealthCheck> Checks { get; set; } = default!;
    public TimeSpan Duration { get; set; }

}
