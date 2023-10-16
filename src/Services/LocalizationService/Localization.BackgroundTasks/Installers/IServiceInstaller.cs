namespace Localization.BackgroundTasks.Installers;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment);
}
