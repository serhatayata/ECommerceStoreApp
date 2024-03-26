using FileService.Api.Attributes;
using FileService.Api.Services.Abstract;
using FileService.Api.Services.Concrete;

namespace FileService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 1)]
public class StartupDIServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IFileUserService, FileUserService>();

        return Task.CompletedTask;
    }
}
