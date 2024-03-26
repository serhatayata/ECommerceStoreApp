using CatalogService.Api.Attributes;
using CatalogService.Api.Mapping;

namespace CatalogService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 9)]
public class MappingServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAutoMapper(typeof(MapProfile).Assembly);

        return Task.CompletedTask;
    }
}
