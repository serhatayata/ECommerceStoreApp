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

    /// <summary>
    /// Navigation property health check execution
    /// </summary>
    public virtual HealthCheckExecution HealthCheckExecution { get; set; }

    /// <summary>
    /// Navigation property id of health check execution
    /// </summary>
    public int HealthCheckExecutionId { get; set; }
}
