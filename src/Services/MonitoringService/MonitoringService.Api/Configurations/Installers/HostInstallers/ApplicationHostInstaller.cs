using MonitoringService.Api.Attributes;
using Serilog;

namespace MonitoringService.Api.Configurations.Installers.HostInstallers;

[InstallerOrder(Order = 1)]
public class ApplicationHostInstaller : IHostInstaller
{
    public void Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        host.UseDefaultServiceProvider((context, options) =>
        {
            options.ValidateOnBuild = false;
            options.ValidateScopes = false;
        });

        host.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("Configurations/StaticFiles/appsettings.json",
                               optional: false,
                               reloadOnChange: true)
                  .AddJsonFile($"Configurations/StaticFiles/appsettings.{hostEnvironment.EnvironmentName}.json",
                               optional: true,
                               reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .Build();
        })
        .ConfigureLogging(s => s.ClearProviders()) // Remove all added providers before
                                                   // https://github.com/serilog/serilog-aspnetcore
        .UseSerilog();
    }
}
