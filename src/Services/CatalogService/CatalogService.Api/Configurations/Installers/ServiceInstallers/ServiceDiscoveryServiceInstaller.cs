using CatalogService.Api.Attributes;
using Consul;

namespace CatalogService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 10)]
public class ServiceDiscoveryServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["ServiceDiscoveryConfig:Address"];
            consulConfig.Address = new Uri(address);
        }));
    }
}
