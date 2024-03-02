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

        host.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("Configurations/Settings/appsettings.json",
                               optional: false,
                               reloadOnChange: true)
                  .AddJsonFile($"Configurations/Settings/appsettings.{hostEnvironment.EnvironmentName}.json",
                               optional: true,
                               reloadOnChange: true)
                  .AddJsonFile($"Configurations/Settings/ocelot.json",
                               optional: false,
                               reloadOnChange: true)
                  .AddJsonFile($"Configurations/Settings/serilog.json",
                             optional: false,
                             reloadOnChange: true) // if same value is used like ConnectionString, this will be the second option (optional)
                  .AddJsonFile($"Configurations/Settings/serilog.{hostEnvironment.EnvironmentName}.json",
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