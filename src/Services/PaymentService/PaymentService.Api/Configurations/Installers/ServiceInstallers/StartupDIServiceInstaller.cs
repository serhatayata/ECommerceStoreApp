using PaymentService.Api.Models.Settings;

namespace PaymentService.Api.Configurations.Installers.ServiceInstallers;

public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.Configure<QueueSettings>(configuration.GetSection($"QueueSettings:{hostEnvironment.EnvironmentName}:RabbitMQ"));
    }
}
