using MonitoringService.Api.Infrastructure.DelegatingHandlers;
using MonitoringService.Api.Models.Settings;

namespace MonitoringService.Api.Extensions;

public static class HttpExtensions
{
    public static void AddHttpClients(this IServiceCollection services, ConfigurationManager configuration)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddScoped<LocalizationAuthorizationDelegatingHandler>();

        #region Service Http Client Identity Server
        var localizationInfo = configuration.GetSection($"ServiceInformation:{env}:LocalizationService").Get<ServiceInformationSettings>();
        var gatewayInfo = configuration.GetSection($"ServiceInformation:{env}:ApiGateway").Get<ServiceInformationSettings>();

        services.AddHttpClient(localizationInfo.Name, config =>
        {
            var baseAddress = localizationInfo.Url;
            config.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<LocalizationAuthorizationDelegatingHandler>();
        #endregion
    }
}
