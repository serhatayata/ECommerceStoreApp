using IdentityServer.Api.Attributes;
using IdentityServer.Api.Configurations.Installers;
using Serilog;

namespace IdentityServer.Api.Configurations.Installers.HostInstallers;

[InstallerOrder(Order = 1)]
public class ApplicationHostInstaller : IHostInstaller
{
    public void Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        host.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("Configurations/Settings/appsettings.json",
                               optional: false,
                               reloadOnChange: true)
                  .AddJsonFile($"Configurations/Settings/appsettings.{hostEnvironment.EnvironmentName}.json",
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