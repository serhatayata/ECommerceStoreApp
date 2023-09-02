namespace MonitoringService.Api.Entities;

public class HealthCheckExecutionEntry
{
    /// <summary>
    /// Id of the execution entry record
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the service
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Status of the execution
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Duration of the execution
    /// </summary>
    public string Duration { get; set; }

    /// <summary>
    /// Tags of the execution
    /// </summary>
    public string Tags { get; set; }
}
