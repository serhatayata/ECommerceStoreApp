namespace Localization.BackgroundTasks.Installers.ServiceInstallers;

public class HttpClientServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        #region CDC Debezium Http Client
        string gatewayClient = configuration.GetSection($"ApiGatewaySettings:{hostEnvironment.EnvironmentName}:Url").Value;

        services.AddHttpClient("apigateway", config =>
        {
            var baseAddress = gatewayClient;
            config.BaseAddress = new Uri(baseAddress);
        });
        #endregion
    }
}
