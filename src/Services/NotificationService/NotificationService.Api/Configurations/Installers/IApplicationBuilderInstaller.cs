namespace NotificationService.Api.Configurations.Installers;

public interface IApplicationBuilderInstaller
{
    void Install(IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration);
}
