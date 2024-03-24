using Localization.BackgroundTasks.Extensions;
using Localization.BackgroundTasks.Installers;
using Localization.BackgroundTasks.Models.Settings;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

var config = ConfigurationExtension.appConfig;

builder.Configuration.AddConfiguration(config);

builder.Services
    .InstallServices(
        configuration,
        environment,
        typeof(IServiceInstaller).Assembly);

builder.Host
    .InstallHost(
    configuration,
    environment,
    typeof(IHostInstaller).Assembly);

builder.Services.Configure<QueueSettings>(configuration.GetSection($"LocalizationQueueSettings:{environment.EnvironmentName}"));
builder.Services.Configure<CacheSettings>(configuration.GetSection($"LocalizationCacheSettings:{environment.EnvironmentName}"));

var app = builder.Build();

app.Start();

app.WaitForShutdown();

public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}