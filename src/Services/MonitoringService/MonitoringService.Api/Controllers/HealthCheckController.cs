using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringService.Api.Attributes;
using MonitoringService.Api.Models.Base.Concrete;
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

    [AuthorizeMultiplePolicy("MonitoringWrite", false)]
    [HttpPost]
    [Route("get-all")]
    public async Task<IActionResult> GetHealthChecksAsync()
    {
        var result = await _diagnosticService.GetAllHealthChecks();
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("MonitoringWrite", false)]
    [HttpPost]
    [Route("get-all-grpc")]
    public async Task<IActionResult> GetHealthChecksGrpcAsync()
    {
        var result = await _diagnosticService.GetAllGrpcHealthChecks();
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("MonitoringWrite", false)]
    [HttpPost]
    [Route("get-grpc")]
    public async Task<IActionResult> GetHealthChecksAsync([FromBody] StringModel model)
    {
        var result = await _diagnosticService.GetGrpcHealthCheck(model.Value);
        return Ok(result);
    }
}