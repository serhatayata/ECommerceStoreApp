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
    public async Task<IActionResult> GetHealthChecksAsync(CancellationToken cancellationToken)
    {
        var result = await _diagnosticService.GetAllHealthChecks(cancellationToken);
        return Ok(result);
    }

    //[AuthorizeMultiplePolicy("MonitoringWrite", false)]
    [HttpPost]
    [Route("get-all-grpc")]
    public async Task<IActionResult> GetHealthChecksGrpcAsync(CancellationToken cancellationToken)
    {
        var result = await _diagnosticService.GetAllGrpcHealthChecks(cancellationToken);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("MonitoringWrite", false)]
    [HttpPost]
    [Route("get-grpc")]
    public async Task<IActionResult> GetHealthCheckGrpcAsync([FromBody] StringModel model, CancellationToken cancellationToken)
    {
        var result = await _diagnosticService.GetGrpcHealthCheck(model.Value, cancellationToken);
        return Ok(result);
    }
}