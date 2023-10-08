using Localization.BackgroundTasks.Services.BackgroundServices;

namespace Localization.BackgroundTasks.Configurations.Installers.ServiceInstallers;

public class BackgroundServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHostedService(service => service.GetRequiredService<ResourceChangeBackgroundService>());
    }
}
