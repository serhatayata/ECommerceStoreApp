
using FileService.Api.Attributes;
using Serilog;

namespace FileService.Api.Configurations.Installers.HostInstallers;

[InstallerOrder(Order = 1)]
public class ApplicationHostInstaller : IHostInstaller
{
    public Task Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var env = hostEnvironment.EnvironmentName.ToLower();

        //host.UseDefaultServiceProvider((context, options) =>
        //{
        //    options.ValidateOnBuild = false;
        //    options.ValidateScopes = false;
        //});

        host.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile($"Configurations/Settings/appsettings.{env}.json",
                               optional: false,
                               reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .Build();
        })
        .ConfigureLogging(s => s.ClearProviders()) // Remove all added providers before
                                                   // https://github.com/serilog/serilog-aspnetcore
        .UseSerilog();

        return Task.CompletedTask;
    }
}