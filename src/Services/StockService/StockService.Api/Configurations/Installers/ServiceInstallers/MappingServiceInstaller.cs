using StockService.Api.Mapping;

namespace StockService.Api.Configurations.Installers.ServiceInstallers;

public class MappingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAutoMapper(typeof(MapProfile).Assembly);
    }
}
