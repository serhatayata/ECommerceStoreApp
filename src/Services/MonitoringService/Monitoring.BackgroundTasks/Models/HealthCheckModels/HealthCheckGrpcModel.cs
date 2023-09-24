using Monitoring.BackgroundTasks.Models.Enums;

namespace Monitoring.BackgroundTasks.Models.HealthCheckModels;

public class HealthCheckGrpcModel
{    
     /// <summary>
     /// Name of the service
     /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// Uri of the service
    /// </summary>
    public string ServiceUri { get; set; }

    /// <summary>
    /// Status of the service
    /// </summary>
    public HealthCheckGrpcStatus Status { get; set; }
}
