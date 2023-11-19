using StockService.Api.Configurations.Installers.WebApplicationInstallers;

namespace StockService.Api.Extensions;

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
