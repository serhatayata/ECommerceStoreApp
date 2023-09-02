namespace MonitoringService.Api.Entities;

public class HealthCheckConfiguration
{
    /// <summary>
    /// Id of the configuration
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Uri of the service
    /// </summary>
    public string Uri { get; set; }

    /// <summary>
    /// Name of the service
    /// </summary>
    public string Name { get; set; }
}
