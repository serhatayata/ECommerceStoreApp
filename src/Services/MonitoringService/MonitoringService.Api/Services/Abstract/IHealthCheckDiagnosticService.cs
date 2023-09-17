using MonitoringService.Api.Models.HealthCheckModels;

namespace MonitoringService.Api.Services.Abstract;

public interface IHealthCheckDiagnosticService
{
    /// <summary>
    /// Get all health check for all services
    /// </summary>
    /// <returns><see cref="List{T}></returns>
    Task<List<HealthCheckModel>> GetAllHealthChecks();

    /// <summary>
    /// Get all health check for all services
    /// </summary>
    /// <returns><see cref="List{T}></returns>
    Task<List<HealthCheckModel>> GetAllGrpcHealthChecks();

    /// <summary>
    /// Get health check specified services
    /// </summary>
    /// <returns><see cref="HealthCheckModel"/></returns>
    Task<HealthCheckModel> GetHealthCheck(string serviceName);

    /// <summary>
    /// Get health check specified services
    /// </summary>
    /// <returns><see cref="HealthCheckModel"/></returns>
    Task<HealthCheckModel> GetGrpcHealthCheck(string serviceName);
}
