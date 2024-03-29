using FileService.Api.Attributes;

namespace FileService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 4)]
public class EndpointServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddEndpointsApiExplorer();

        return Task.CompletedTask;
    }
}
