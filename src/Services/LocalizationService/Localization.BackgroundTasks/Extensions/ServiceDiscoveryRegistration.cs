using Localization.BackgroundTasks.Installers.ApplicationBuilderInstallers;

namespace IdentityServer.Api.Extensions;

public static class ServiceDiscoveryRegistration
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
