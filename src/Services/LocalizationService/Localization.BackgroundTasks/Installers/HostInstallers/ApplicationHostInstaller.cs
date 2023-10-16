using Serilog;

namespace Localization.BackgroundTasks.Installers.HostInstallers;

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
            config.AddJsonFile("Configurations/appsettings.json",
                               optional: false,
                               reloadOnChange: true)
                  .AddJsonFile($"Configurations/appsettings.{hostEnvironment.EnvironmentName}.json",
                               optional: true,
                               reloadOnChange: true)
                  .Build();
        })
        .ConfigureLogging(s => s.ClearProviders()) // Remove all added providers before
                                                   // https://github.com/serilog/serilog-aspnetcore
        .UseSerilog();
    }
}
