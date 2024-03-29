using MonitoringService.Api.Mapping;
using MonitoringService.Api.Attributes;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 5)]
public class MappingServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAutoMapper(typeof(MapProfile).Assembly);

        return Task.CompletedTask;
    }
}
