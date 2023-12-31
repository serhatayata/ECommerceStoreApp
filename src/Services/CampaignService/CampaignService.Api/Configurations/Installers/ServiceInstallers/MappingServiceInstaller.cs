namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

using CampaignService.Api.Mapping;

public class MappingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAutoMapper(typeof(MapProfile).Assembly);
    }
}
