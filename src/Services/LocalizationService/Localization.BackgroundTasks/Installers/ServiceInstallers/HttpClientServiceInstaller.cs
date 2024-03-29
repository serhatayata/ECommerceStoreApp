using Localization.BackgroundTasks.Attributes;

namespace Localization.BackgroundTasks.Installers.ServiceInstallers;

[InstallerOrder(Order = 2)]
public class HttpClientServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        #region CDC Debezium Http Client
        string gatewayClient = configuration.GetSection($"ApiGatewaySettings:{hostEnvironment.EnvironmentName}:Url").Value;

        services.AddHttpClient("apigateway", config =>
        {
            var baseAddress = gatewayClient;
            config.BaseAddress = new Uri(baseAddress);
        });
        #endregion

        return Task.CompletedTask;
    }
}
