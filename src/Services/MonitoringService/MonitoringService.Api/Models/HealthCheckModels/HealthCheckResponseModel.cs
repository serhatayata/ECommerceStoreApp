namespace MonitoringService.Api.Models.HealthCheckModels;

public class HealthCheckResponseModel
{
    /// <summary>
    /// Status of the service
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Duration of the service health check time
    /// </summary>
    public string Duration { get; set; }

    /// <summary>
    /// Entries of the health check
    /// </summary>
    public List<HealthCheckEntry> Info { get; set; }
}
