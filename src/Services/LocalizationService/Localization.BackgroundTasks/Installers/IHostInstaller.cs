namespace Localization.BackgroundTasks.Installers;

public interface IHostInstaller
{
    Task Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment);
}
