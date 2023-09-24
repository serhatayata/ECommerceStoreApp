using MonitoringService.Api.Models.HealthCheckModels;
using MonitoringService.Api.Utilities.Results;

namespace MonitoringService.Api.Services.HealthCheck.Abstract;

public interface IHealthCheckDiagnosticService
{
    /// <summary>
    /// Get all health check for all services
    /// </summary>
    /// <returns><see cref="DataResult{T}></returns>
    Task<DataResult<List<HealthCheckModel>>> GetAllHealthChecks(CancellationToken cancellationToken);

    /// <summary>
    /// Get all health check for all services
    /// </summary>
    /// <returns><see cref="DataResult{T}></returns>
    Task<DataResult<List<GrpcHealthCheckModel>>> GetAllGrpcHealthChecks(CancellationToken cancellationToken);

    /// <summary>
    /// Get health check specified services
    /// </summary>
    /// <returns><see cref="DataResult{T}"/></returns>
    Task<DataResult<HealthCheckModel>> GetHealthCheck(string serviceName, CancellationToken cancellationToken);

    /// <summary>
    /// Get health check specified services
    /// </summary>
    /// <returns><see cref="DataResult{T}"/></returns>
    Task<DataResult<GrpcHealthCheckModel>> GetGrpcHealthCheck(string serviceName, CancellationToken cancellationToken);
}
