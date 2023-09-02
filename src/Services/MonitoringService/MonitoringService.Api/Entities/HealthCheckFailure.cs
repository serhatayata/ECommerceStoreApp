namespace MonitoringService.Api.Entities;

public class HealthCheckFailure
{
    /// <summary>
    /// Id of the failure record
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the service
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// Creation date of the record
    /// </summary>
    public DateTime CreateDate { get; set; }
}
