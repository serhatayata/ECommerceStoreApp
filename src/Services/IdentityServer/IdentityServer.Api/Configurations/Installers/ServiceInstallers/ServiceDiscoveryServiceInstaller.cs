using Consul;
using IdentityServer.Api.Attributes;
using IdentityServer.Api.Configurations.Installers;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 9)]
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
