using Grpc.Health.V1;
using Grpc.Net.Client;
using MonitoringService.Api.Extensions;
using MonitoringService.Api.Models.HealthCheckModels;
using MonitoringService.Api.Models.Settings;
using MonitoringService.Api.Services.Abstract;
using MonitoringService.Api.Utilities.Results;
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

    public async Task<DataResult<List<HealthCheckModel>>> GetAllHealthChecks(CancellationToken cancellationToken)
    {
        var serviceInformation = _configuration.GetSection($"ServiceInformation:{this.Env}").Get<ServiceInformationSettings[]>();
        if (serviceInformation == null)
            return new ErrorDataResult<List<HealthCheckModel>>(null);

        var responseModel = new List<HealthCheckModel>();

        foreach (var info in serviceInformation)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(info.Name);

                var requestUrl = string.Join(string.Empty, info.Url, info.UrlSuffix);
                var httpResponse = await client.PostGetResponseAsync<HealthCheckResponseModel, string>(requestUrl, null, cancellationToken);

                if (httpResponse == null)
                    continue;

                responseModel.Add(new HealthCheckModel()
                {
                    ServiceName = info.Name,
                    Status = httpResponse.Status,
                    ServiceUri = info.Url,
                    TotalDuration = httpResponse.Duration,
                    Info = httpResponse.Info,
                });
            }
            catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "ERROR health check: {ExceptionMessage} - Method : {ClassName}.{MethodName} - Service name : {ServiceName}",
                                 ex.Message,
                                 nameof(HealthCheckDiagnosticService),
                                 MethodBase.GetCurrentMethod()?.Name,
                                 info.Name);

                responseModel.Add(new HealthCheckModel()
                {
                    ServiceName = info.Name
                });
            }
        }

        return new SuccessDataResult<List<HealthCheckModel>>(responseModel); ;
    }

    public async Task<DataResult<HealthCheckModel>> GetHealthCheck(string serviceName, CancellationToken cancellationToken)
    {
        var serviceInformation = _configuration.GetSection($"ServiceInformation:{this.Env}").Get<ServiceInformationSettings[]>();
        if (serviceInformation == null)
            return new ErrorDataResult<HealthCheckModel>(null);

        var currentService = serviceInformation.FirstOrDefault(s => s.Name == serviceName);
        if (currentService == null)
            return new ErrorDataResult<HealthCheckModel>(null);

        try
        {
            var client = _httpClientFactory.CreateClient(currentService.Name);

            var requestUrl = string.Join(string.Empty,
                                         currentService.Url,
                                         currentService.UrlSuffix);

            var httpResponse = await client.PostGetResponseAsync<HealthCheckResponseModel, string>(requestUrl, null, cancellationToken);

            if (httpResponse == null)
                return new ErrorDataResult<HealthCheckModel>(null);

            var result = new HealthCheckModel()
            {
                ServiceName = currentService.Name,
                Status = httpResponse.Status,
                ServiceUri = currentService.Url,
                TotalDuration = httpResponse.Duration,
                Info = httpResponse.Info,
            };

            return new SuccessDataResult<HealthCheckModel>(result);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(ex, "ERROR health check: {ExceptionMessage} - Method : {ClassName}.{MethodName} - Service name : {ServiceName}",
                             ex.Message,
                             nameof(HealthCheckDiagnosticService),
                             MethodBase.GetCurrentMethod()?.Name,
                             currentService.Name);

            return new ErrorDataResult<HealthCheckModel>(null);
        }
    }

    public async Task<DataResult<List<GrpcHealthCheckModel>>> GetAllGrpcHealthChecks(CancellationToken cancellationToken)
    {
        var responseModel = new List<GrpcHealthCheckModel>();

        try
        {
            var serviceInformation = _configuration.GetSection($"GrpcServiceInformation:{this.Env}").Get<ServiceInformationSettings[]>();
            if (serviceInformation == null)
                return new ErrorDataResult<List<GrpcHealthCheckModel>>(responseModel);

            foreach (var service in serviceInformation)
            {
                var channel = GrpcChannel.ForAddress(service.Url);
                var client = new Health.HealthClient(channel);

                var response = await client.CheckAsync(request: new HealthCheckRequest(), cancellationToken: cancellationToken);
                var status = response.Status;

                responseModel.Add(new GrpcHealthCheckModel(service.Name, service.Url, status));
            }

            return new SuccessDataResult<List<GrpcHealthCheckModel>>(responseModel);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(ex, "ERROR grpc health check: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                 ex.Message,
                 nameof(HealthCheckDiagnosticService),
                 MethodBase.GetCurrentMethod()?.Name);

            return new ErrorDataResult<List<GrpcHealthCheckModel>>(responseModel);
        }
    }

    public async Task<DataResult<GrpcHealthCheckModel>> GetGrpcHealthCheck(string serviceName, CancellationToken cancellationToken)
    {
        try
        {
            var serviceInformation = _configuration.GetSection($"GrpcServiceInformation:{this.Env}").Get<ServiceInformationSettings[]>();
            if (serviceInformation == null)
                return new ErrorDataResult<GrpcHealthCheckModel>(null);

            var service = serviceInformation.FirstOrDefault(s => s.Name == serviceName);
            if (service == null)
                return new ErrorDataResult<GrpcHealthCheckModel>(null);

            var channel = GrpcChannel.ForAddress(service.Url);
            var client = new Health.HealthClient(channel);

            var response = await client.CheckAsync(request: new HealthCheckRequest(), cancellationToken: cancellationToken);
            if(response == null)
                return new ErrorDataResult<GrpcHealthCheckModel>(null);

            var status = response.Status;

            var result = new GrpcHealthCheckModel(service.Name, service.Url, status);
            return new SuccessDataResult<GrpcHealthCheckModel>(result);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(ex, "ERROR grpc health check: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                 ex.Message,
                 nameof(HealthCheckDiagnosticService),
                 MethodBase.GetCurrentMethod()?.Name);

            return new ErrorDataResult<GrpcHealthCheckModel>(null);
        }
    }
}
