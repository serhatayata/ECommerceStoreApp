using CampaignService.Api.Configurations.Installers;
using GraphQL.Types;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

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

builder.Services.Configure<KestrelServerOptions>(opt =>
{
    opt.AllowSynchronousIO = true;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.InstallWebApp(app.Lifetime,
                  configuration,
                  typeof(IWebApplicationInstaller).Assembly);

app.MapControllers();
app.Run();

public partial class Program
{
    public static string appName = Assembly.GetExecutingAssembly().GetName().Name;
}