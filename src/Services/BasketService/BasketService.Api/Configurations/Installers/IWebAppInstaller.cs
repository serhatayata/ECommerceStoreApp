namespace BasketService.Api.Configurations.Installers;

public interface IWebAppInstaller
{
    void Install(IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration);
}
