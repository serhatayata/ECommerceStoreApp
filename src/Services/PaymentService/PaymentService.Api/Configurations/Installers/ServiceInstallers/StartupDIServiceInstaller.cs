using PaymentService.Api.Models.Settings;

namespace PaymentService.Api.Configurations.Installers.ServiceInstallers;

public class StartupDIServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        return Task.CompletedTask;
    }
}
