using Microsoft.AspNetCore.Http;
using MonitoringService.Api.Extensions;
using MonitoringService.Api.Models.HealthCheckModels;
using MonitoringService.Api.Models.Settings;
using MonitoringService.Api.Services.Abstract;
using System.Reflection;

namespace MonitoringService.Api.Services.Concrete;

public class HealthCheckDiagnosticService : BaseService, IHealthCheckDiagnosticService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HealthCheckDiagnosticService> _logger;

    public HealthCheckDiagnosticService(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<HealthCheckDiagnosticService> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<HealthCheckModel>> GetAllHealthChecks()
    {
        var serviceInformation = _configuration.GetSection($"ServiceInformation:{this.Env}").Get<ServiceInformationSettings[]>();
        if (serviceInformation == null)
            return null;

        var responseModel = new List<HealthCheckModel>();

        foreach (var info in serviceInformation)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(info.Name);

                var requestUrl = string.Join(string.Empty, info.Url, info.UrlSuffix);
                var httpResponse = await client.PostGetResponseAsync<HealthCheckResponseModel, string>(requestUrl, null);

                if (httpResponse == null)
                    continue;

                responseModel.Add(new HealthCheckModel()
                {
                    ServiceName = info.Name,
                    Status = httpResponse.Status,
                    TotalDuration = httpResponse.Duration,
                    Info = httpResponse.Info,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR health check: {ExceptionMessage} - Method : {ClassName}.{MethodName} - Service name : {ServiceName}",
                                 ex.Message,
                                 nameof(GetAllHealthChecks),
                                 MethodBase.GetCurrentMethod()?.Name,
                                 info.Name);

                responseModel.Add(new HealthCheckModel()
                {
                    ServiceName = info.Name
                });
            }
        }

        return responseModel;
    }

    public async Task<HealthCheckModel> GetHealthCheck(string serviceName)
    {
        var serviceInformation = _configuration.GetSection($"ServiceInformation:{this.Env}").Get<ServiceInformationSettings[]>();
        if (serviceInformation == null)
            return null;

        var currentService = serviceInformation.FirstOrDefault(s => s.Name == serviceName);
        if (currentService == null)
            return null;

        try
        {
            var client = _httpClientFactory.CreateClient(currentService.Name);

            var requestUrl = string.Join(string.Empty,
                                         currentService.Url,
                                         currentService.UrlSuffix);

            var httpResponse = await client.PostGetResponseAsync<HealthCheckResponseModel, string>(requestUrl, null);

            if (httpResponse == null)
                return null;

            return new HealthCheckModel()
            {
                ServiceName = currentService.Name,
                Status = httpResponse.Status,
                TotalDuration = httpResponse.Duration,
                Info = httpResponse.Info,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR health check: {ExceptionMessage} - Method : {ClassName}.{MethodName} - Service name : {ServiceName}",
                             ex.Message,
                             nameof(GetAllHealthChecks),
                             MethodBase.GetCurrentMethod()?.Name,
                             currentService.Name);

            return null
        }
    }
}
