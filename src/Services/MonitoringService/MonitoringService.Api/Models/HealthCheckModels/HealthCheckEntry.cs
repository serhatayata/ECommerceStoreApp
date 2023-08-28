using System.Text.Json.Serialization;

namespace MonitoringService.Api.Models.HealthCheckModels;

public class HealthCheckEntry
{    
    /// <summary>
    /// Key of the entry
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Description of the entry
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Duration of the entry
    /// </summary>
    public string Duration { get; set; }

    /// <summary>
    /// Status of the entry
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Error of the entry
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Data of the health check
    /// </summary>
    public IReadOnlyDictionary<string, object> Data { get; set; }

    /// <summary>
    /// Tags of the entry
    /// </summary>
    public IEnumerable<string> Tags { get; set; } = new List<string>();

}
