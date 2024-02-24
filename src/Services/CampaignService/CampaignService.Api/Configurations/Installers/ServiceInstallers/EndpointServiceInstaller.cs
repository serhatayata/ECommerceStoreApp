
using CampaignService.Api.Attributes;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 4)]
public class EndpointServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddEndpointsApiExplorer();
    }
}
