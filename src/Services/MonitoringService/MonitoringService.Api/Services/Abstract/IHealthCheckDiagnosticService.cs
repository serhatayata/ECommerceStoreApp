using MonitoringService.Api.Models.HealthCheckModels;
using MonitoringService.Api.Utilities.Results;

namespace MonitoringService.Api.Services.Abstract;

public interface IHealthCheckDiagnosticService
{
    /// <summary>
    /// Get all health check for all services
    /// </summary>
    /// <returns><see cref="DataResult{T}></returns>
    Task<DataResult<List<HealthCheckModel>>> GetAllHealthChecks();

    /// <summary>
    /// Get all health check for all services
    /// </summary>
    /// <returns><see cref="DataResult{T}></returns>
    Task<DataResult<List<GrpcHealthCheckModel>>> GetAllGrpcHealthChecks();

    /// <summary>
    /// Get health check specified services
    /// </summary>
    /// <returns><see cref="DataResult{T}"/></returns>
    Task<DataResult<HealthCheckModel>> GetHealthCheck(string serviceName);

    /// <summary>
    /// Get health check specified services
    /// </summary>
    /// <returns><see cref="DataResult{T}"/></returns>
    Task<DataResult<GrpcHealthCheckModel>> GetGrpcHealthCheck(string serviceName);
}
