namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

public class ApiDocumentationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSwaggerGen();
    }
}
