using Monitoring.BackgroundTasks.Attributes;
using Monitoring.BackgroundTasks.Infrastructure.DelegatingHandlers;

namespace Monitoring.BackgroundTasks.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 3)]
public class HttpClientServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddScoped<MonitoringAuthorizationDelegatingHandler>();

        #region HttpClients
        string monitoringClient = configuration.GetSection($"ServiceInformation:{env}:MonitoringService:Url").Value;

        services.AddHttpClient("monitoring", config =>
        {
            var baseAddress = $"{monitoringClient}";
            config.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<MonitoringAuthorizationDelegatingHandler>();

        #endregion    

        return Task.CompletedTask;
    }
}
