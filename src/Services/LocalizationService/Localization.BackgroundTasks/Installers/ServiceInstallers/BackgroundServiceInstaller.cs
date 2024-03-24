using Localization.BackgroundTasks.Attributes;
using Localization.BackgroundTasks.Services.BackgroundServices;

namespace Localization.BackgroundTasks.Installers.ServiceInstallers;

[InstallerOrder(Order = 2)]
public class BackgroundServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHostedService(service => service.GetRequiredService<ResourceChangeBackgroundService>());
    }
}
