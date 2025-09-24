namespace HealthService.Models;

public class HealthCheck
{
    public string? Status { get; set; } = null;
    public string? Component { get; set; } = null;
    public string? Description { get; set; } = null;
    public string? ExceptionMessage { get; set; } = null;
    public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
    public TimeSpan Duration { get; set; }
    public IReadOnlyDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

}