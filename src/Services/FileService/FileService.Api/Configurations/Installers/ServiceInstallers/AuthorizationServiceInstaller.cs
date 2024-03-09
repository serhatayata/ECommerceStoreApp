using FileService.Api.Attributes;

namespace FileService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 11)]
public class AuthorizationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        
    }
}
