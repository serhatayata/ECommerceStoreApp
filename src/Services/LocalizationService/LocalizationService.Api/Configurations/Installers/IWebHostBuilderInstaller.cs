namespace LocalizationService.Api.Configurations.Installers;

public interface IWebHostBuilderInstaller
{
    Task Install(ConfigureWebHostBuilder builder, IWebHostEnvironment hostEnv, IConfiguration configuration);
}
