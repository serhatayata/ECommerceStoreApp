using MonitoringService.Api.Attributes;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 8)]
public class EndpointServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddEndpointsApiExplorer();

        return Task.CompletedTask;
    }
}
