using IdentityServer.Api.Attributes;
using IdentityServer.Api.Handlers.ApiTokenHandlers;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 5)]
public class HttpClientServiceInstaller : IServiceInstaller
{
    public void Install(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment hostEnvironment)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddScoped<LocalizationRequestTokenHandler>();

        #region HttpClients
        string gatewayClient = configuration.GetSection($"SourceOriginSettings:{env}:Gateway").Value;

        services.AddHttpClient("gateway-specific", config =>
        {
            var baseAddress = $"{gatewayClient}";
            config.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<LocalizationRequestTokenHandler>();

        services.AddHttpClient("gateway", config =>
        {
            var baseAddress = $"{gatewayClient}";
            config.BaseAddress = new Uri(baseAddress);
        });
        #endregion    
    }
}
