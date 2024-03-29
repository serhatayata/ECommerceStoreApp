namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

public class HttpServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return Task.CompletedTask;
    }
}
