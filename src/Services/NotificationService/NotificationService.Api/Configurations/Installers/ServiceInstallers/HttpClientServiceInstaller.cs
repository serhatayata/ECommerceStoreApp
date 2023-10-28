using NotificationService.Api.Infrastructure.DelegatingHandlers;
using NotificationService.Api.Models.Settings;

namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

public class HttpClientServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddScoped<LocalizationAuthorizationDelegatingHandler>();

        #region Service Http Client Identity Server
        var localizationInfo = configuration.GetSection($"ServiceInformation:{env}:LocalizationService").Get<ServiceInformationSettings>();

        services.AddHttpClient(localizationInfo.Name, config =>
        {
            var baseAddress = localizationInfo.Url;
            config.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<LocalizationAuthorizationDelegatingHandler>();
        #endregion
    }
}