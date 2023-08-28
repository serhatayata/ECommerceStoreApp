using MonitoringService.Api.Models.HealthCheckModels;

namespace MonitoringService.Api.Services.Abstract;

public interface IHealthCheckDiagnosticService
{
    Task<List<HealthCheckModel>> GetAllHealthChecks();
}
