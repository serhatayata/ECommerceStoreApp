using OrderService.Api.Configurations.Installers.WebApplicationInstallers;

namespace OrderService.Api.Extensions;

public static class ServiceDiscoveryExtensions
{
    public static void InstallServiceDiscovery(
        this WebApplication app, 
        IHostApplicationLifetime lifeTime, 
        IConfiguration configuration)
    {
        var serviceDiscoveryInstaller = new ServiceDiscoveryWebAppInstaller();
        serviceDiscoveryInstaller.Install(app, lifeTime, configuration);
    }
}
