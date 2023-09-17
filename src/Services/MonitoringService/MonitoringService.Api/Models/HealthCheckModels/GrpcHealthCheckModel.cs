using Grpc.Health.V1;

namespace MonitoringService.Api.Models.HealthCheckModels;

public class GrpcHealthCheckModel
{
    public GrpcHealthCheckModel(
        string serviceName, 
        string serviceUri, 
        HealthCheckResponse.Types.ServingStatus status)
    {
        ServiceName = serviceName;
        ServiceUri = serviceUri;
        Status = status;
    }

    public GrpcHealthCheckModel()
    {
        
    }

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
    public HealthCheckResponse.Types.ServingStatus Status { get; set; }
}
