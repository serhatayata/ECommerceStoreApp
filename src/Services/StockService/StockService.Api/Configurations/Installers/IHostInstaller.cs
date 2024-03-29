namespace StockService.Api.Configurations.Installers;

public interface IHostInstaller
{
    Task Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment);
}
