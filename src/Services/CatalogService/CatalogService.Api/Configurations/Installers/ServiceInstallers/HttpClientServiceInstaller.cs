using CatalogService.Api.Attributes;
using CatalogService.Api.Infrastructure.Handlers.ApiTokenHandlers;

namespace CatalogService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 3)]
public class HttpClientServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddScoped<LocalizationRequestTokenHandler>();

        #region HttpClients
        string gatewayClient = configuration.GetSection($"SourceOriginSettings:Gateway").Value;

        services.AddHttpClient("gateway", config =>
        {
            var baseAddress = $"{gatewayClient}";
            config.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<LocalizationRequestTokenHandler>();

        #endregion
    }
}
