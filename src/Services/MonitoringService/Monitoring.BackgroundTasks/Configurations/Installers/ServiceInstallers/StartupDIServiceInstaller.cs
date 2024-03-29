using Monitoring.BackgroundTasks.Attributes;
using Monitoring.BackgroundTasks.Models.Settings;
using Monitoring.BackgroundTasks.Services.Abstract;
using Monitoring.BackgroundTasks.Services.Concrete;

namespace Monitoring.BackgroundTasks.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 1)]
public class StartupDIServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddScoped<IClientCredentialsTokenService, ClientCredentialsTokenService>();

        services.Configure<HealthCheckSaveSettings>(configuration.GetSection("Settings:HealthCheckSave"));

        return Task.CompletedTask;
    }
}
