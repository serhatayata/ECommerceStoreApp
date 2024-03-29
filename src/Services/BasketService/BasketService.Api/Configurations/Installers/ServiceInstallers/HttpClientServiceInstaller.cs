
using BasketService.Api.Infrastructure.Handlers.ApiTokenHandlers;

namespace BasketService.Api.Configurations.Installers.ServiceInstallers;

public class HttpClientServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddScoped<LocalizationRequestTokenHandler>();

        #region HttpClients
        string gatewayClient = configuration.GetSection($"SourceOriginSettings:{env}:Gateway").Value;

        services.AddHttpClient("gateway", config =>
        {
            var baseAddress = $"{gatewayClient}";
            config.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<LocalizationRequestTokenHandler>();

        #endregion

        return Task.CompletedTask;
    }
}
