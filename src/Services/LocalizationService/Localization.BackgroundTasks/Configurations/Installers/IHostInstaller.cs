namespace Localization.BackgroundTasks.Configurations.Installers;

public interface IHostInstaller
{
    void Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment);
}
