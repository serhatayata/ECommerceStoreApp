namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

public class SwaggerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSwaggerGen();
    }
}
