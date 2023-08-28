namespace MonitoringService.Api.Models.HealthCheckModels;

public class HealthCheckModel
{
    /// <summary>
    /// Name of the service
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// Status of the service
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Duration of the service health check time
    /// </summary>
    public string TotalDuration{ get; set; }

    /// <summary>
    /// Entries of the health check
    /// </summary>
    public List<HealthCheckEntry> Info { get; set; } = new List<HealthCheckEntry>();
}
