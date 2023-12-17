using CatalogService.Api.Configurations.Installers;
using CatalogService.Api.Extensions;
using CatalogService.Api.Extensions.Middlewares;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

builder.WebHost
    .InstallWebAppBuilder(
        environment,
        configuration,
        typeof(IWebHostBuilderInstaller).Assembly
    );

builder.Host
    .InstallHost(
    configuration,
    environment,
    typeof(IHostInstaller).Assembly);

builder.Services
    .InstallServices(
        configuration,
        environment,
        typeof(IServiceInstaller).Assembly);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    ResponseWriter = HealthCheckExtensions.WriteResponse
});

app.InstallWebApp(app.Lifetime,
                  configuration,
                  typeof(IWebAppInstaller).Assembly);

app.InstallApplicationBuilder(app.Lifetime,
                              configuration,
                              typeof(IApplicationBuilderInstaller).Assembly);

app.MapControllers();

#region CUSTOM MIDDLEWARES
app.UseResponseTimeMiddleware();
#endregion

app.Start();
app.InstallServiceDiscovery(app.Lifetime, configuration);
app.WaitForShutdown();

public partial class Program
{
    public static string appName = Assembly.GetExecutingAssembly().GetName().Name;
}

