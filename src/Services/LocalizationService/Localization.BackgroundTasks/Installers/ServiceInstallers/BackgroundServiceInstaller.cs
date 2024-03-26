using Localization.BackgroundTasks.Attributes;
using Localization.BackgroundTasks.Services.BackgroundServices;

namespace Localization.BackgroundTasks.Installers.ServiceInstallers;

[InstallerOrder(Order = 2)]
public class BackgroundServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHostedService(service => service.GetRequiredService<ResourceChangeBackgroundService>());

        return Task.CompletedTask;
    }
}
