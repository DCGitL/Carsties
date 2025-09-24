using HealthService.Models;

namespace HealthService;

public interface IHealthStatusService
{
    Task<HealthCheckResponse> GetHealthStatusAsync(string? tag);
}
