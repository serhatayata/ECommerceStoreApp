using Consul;

namespace PaymentService.Api.Configurations.Installers.ServiceInstallers;

public class ServiceDiscoveryServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["ServiceDiscoveryConfig:Address"];
            consulConfig.Address = new Uri(address);
        }));

        return Task.CompletedTask;
    }
}
