using LocalizationService.Api.Configurations.Installers;
using LocalizationService.Api.Extensions;
using LocalizationService.Api.Utilities.IoC;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

ServiceTool.Create(builder.Services);

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

app.ConfigureCustomExceptionMiddleware();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.InstallWebApp(app.Lifetime,
                  configuration,
                  typeof(IWebApplicationInstaller).Assembly);

app.InstallApplicationBuilder(app.Lifetime,
                              configuration,
                              typeof(IApplicationBuilderInstaller).Assembly);

app.MapControllers();

app.Start();
app.InstallServiceDiscovery(app.Lifetime, configuration);
app.WaitForShutdown();