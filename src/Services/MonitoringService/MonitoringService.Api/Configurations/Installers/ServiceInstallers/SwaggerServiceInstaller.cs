using MonitoringService.Api.Attributes;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 6)]
public class SwaggerServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSwaggerGen();

        return Task.CompletedTask;
    }
}
