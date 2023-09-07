using Monitoring.BackgroundTasks.Extensions;
using Monitoring.BackgroundTasks.Models.Settings;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Host.AddHostExtensions(environment);

#region LOG
builder.Services.AddLogConfiguration();
#endregion
#region Http
builder.Services.AddHttpClients(configuration);
#endregion
#region Configurations
builder.Services.Configure<HealthCheckSaveSettings>(configuration.GetSection("Settings:HealthCheckSave"));
#endregion
#region Scheduling
builder.Services.AddJobConfigurations(configuration);
builder.Services.AddQuartzHostedService(q =>
{
    q.WaitForJobsToComplete = true;
});
#endregion

var app = builder.Build();

app.Run();

public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}