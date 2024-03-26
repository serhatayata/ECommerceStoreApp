using LocalizationService.Api.Attributes;

namespace LocalizationService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 14)]
public class EndpointServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddEndpointsApiExplorer();

        return Task.CompletedTask;
    }
}
