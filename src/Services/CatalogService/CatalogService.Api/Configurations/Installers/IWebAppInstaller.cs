namespace CatalogService.Api.Configurations.Installers;

public interface IWebAppInstaller
{
    void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration);
}
