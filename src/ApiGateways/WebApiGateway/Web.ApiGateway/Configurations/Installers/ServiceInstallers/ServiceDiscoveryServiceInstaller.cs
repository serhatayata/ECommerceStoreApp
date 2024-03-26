using Consul;
using Web.ApiGateway.Attributes;

namespace Web.ApiGateway.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 11)]
public class ServiceDiscoveryServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["ConsulConfig:Address"];
            consulConfig.Address = new Uri(address);
        }));

        return Task.CompletedTask;
    }
}
