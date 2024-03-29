
using CampaignService.Api.Attributes;
using Serilog;

namespace CampaignService.Api.Configurations.Installers.HostInstallers;

[InstallerOrder(Order = 1)]
public class ApplicationHostInstaller : IHostInstaller
{
    public Task Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
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
                  .AddEnvironmentVariables()
                  .Build();
        })
        .ConfigureLogging(s => s.ClearProviders()) // Remove all added providers before
                                                   // https://github.com/serilog/serilog-aspnetcore
        .UseSerilog();
        //.UseSerilog((hostingContext, loggerConfiguration) =>
        //{
        //    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
        //});

        return Task.CompletedTask;
    }
}
