namespace LocalizationService.Api.Configurations.Installers;

public interface IWebHostBuilderInstaller
{
    void Install(ConfigureWebHostBuilder builder, IWebHostEnvironment hostEnv, IConfiguration configuration);
}
