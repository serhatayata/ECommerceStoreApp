using Microsoft.AspNetCore.Mvc;
using MonitoringService.Api.Models.Base.Concrete;
using MonitoringService.Api.Models.HealthCheckModels;
using MonitoringService.Api.Services.Abstract;

namespace MonitoringService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    private readonly IHealthCheckDiagnosticService _diagnosticService;

    public HealthCheckController(
        IHealthCheckDiagnosticService diagnosticService)
    {
        _diagnosticService = diagnosticService;
    }

    [HttpPost]
    [Route("healthchecks-all")]
    public async Task<IActionResult> GetHealthChecksAsync()
    {
        var result = await _diagnosticService.GetAllHealthChecks();
        return Ok(result);
    }

    [HttpPost]
    [Route("healthcheck")]
    public async Task<IActionResult> GetHealthChecksAsync([FromBody] StringModel model)
    {
        var result = await _diagnosticService.GetHealthCheck(model.Value);
        return Ok(result);
    }
}