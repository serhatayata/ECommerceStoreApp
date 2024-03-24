using Monitoring.BackgroundTasks.Attributes;

namespace Monitoring.BackgroundTasks.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 2)]
public class HttpServiceInstaller : IServiceInstaller
{
    public void Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}
