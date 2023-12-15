namespace BasketService.Api.Configurations.Installers;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment);
}
