using NotificationService.Api.Configurations.Installers.ApplicationBuilderInstallers;

namespace NotificationService.Api.Extensions;

public static class ServiceDiscoveryExtensions
{
    public static void InstallServiceDiscovery(
    this WebApplication app,
    IHostApplicationLifetime lifeTime,
    IConfiguration configuration)
    {
        var serviceDiscoveryInstaller = new ServiceDiscoveryApplicationBuilderInstaller();
        serviceDiscoveryInstaller.Install(app, lifeTime, configuration);
    }
}
