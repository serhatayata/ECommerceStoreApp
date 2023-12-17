namespace LocalizationService.Api.Configurations.Installers;

public interface IWebApplicationInstaller
{
    void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration);
}
