using CatalogService.Api.Attributes;

namespace CatalogService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 4)]
public class EndpointServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddEndpointsApiExplorer();

        return Task.CompletedTask;
    }
}
