namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

using CampaignService.Api.Attributes;
using CampaignService.Api.Mapping;

[InstallerOrder(Order = 8)]
public class MappingServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAutoMapper(typeof(MapProfile).Assembly);

        return Task.CompletedTask;
    }
}
