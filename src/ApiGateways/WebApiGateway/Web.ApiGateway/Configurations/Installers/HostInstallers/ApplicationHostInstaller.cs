using Serilog;
using Web.ApiGateway.Attributes;

namespace Web.ApiGateway.Configurations.Installers.HostInstallers;

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

        var envExtended = hostEnvironment.EnvironmentName.ToLower();

        host.ConfigureAppConfiguration(config =>
        {

            config.AddJsonFile($"Configurations/Settings/appsettings.{envExtended}.json",
                               optional: true,
                               reloadOnChange: true)
                  .AddJsonFile($"Configurations/Settings/ocelot.{envExtended}.json",
                               optional: false,
                               reloadOnChange: true)
                  .AddJsonFile($"Configurations/Settings/serilog.{envExtended}.json",
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