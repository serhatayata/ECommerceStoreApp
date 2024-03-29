using FileService.Api.Attributes;

namespace FileService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 11)]
public class AuthorizationServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        return Task.CompletedTask;   
    }
}
