namespace Localization.BackgroundTasks.Installers;

public interface IApplicationBuilderInstaller
{
    void Install(IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration);
}
