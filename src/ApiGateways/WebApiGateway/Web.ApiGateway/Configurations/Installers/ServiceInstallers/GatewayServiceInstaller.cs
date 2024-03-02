using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Attributes;

namespace Web.ApiGateway.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 10)]
public class GatewayServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddOcelot().AddConsul();
    }
}
