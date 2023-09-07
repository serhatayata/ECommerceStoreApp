using MonitoringService.Api.Models.Enums;

namespace MonitoringService.Api.Entities;

public class HealthCheckExecution
{
    /// <summary>
    /// Id of the execution record
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Status of the execution
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Execution date
    /// </summary>
    public DateTime ExecutionDate { get; set; }

    /// <summary>
    /// Uri of the service
    /// </summary>
    public string Uri { get; set; }

    /// <summary>
    /// Name of the service
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// Navigation property of health check execution entries
    /// </summary>
    public virtual ICollection<HealthCheckExecutionEntry> HealthCheckExecutionEntries { get; set; }
}
