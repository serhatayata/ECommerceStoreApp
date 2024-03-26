using FileService.Api.Attributes;
using FileService.Api.Mappings;

namespace FileService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 8)]
public class MappingServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAutoMapper(typeof(MapProfile).Assembly);

        return Task.CompletedTask;
    }
}
