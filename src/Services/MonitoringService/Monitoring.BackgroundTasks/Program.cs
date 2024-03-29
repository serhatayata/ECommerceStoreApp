using Monitoring.BackgroundTasks.Configurations.Installers;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

await builder.Host
       .InstallHost(
         configuration,
         environment,
         typeof(IHostInstaller).Assembly);

await builder.Services
       .InstallServices(
           configuration,
           environment,
           typeof(IServiceInstaller).Assembly);

var app = builder.Build();

app.Run();

public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}