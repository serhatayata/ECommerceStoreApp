using Localization.BackgroundTasks.Extensions;
using Localization.BackgroundTasks.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

var config = ConfigurationExtension.appConfig;

builder.Configuration.AddConfiguration(config);

string defaultConnString = configuration.GetConnectionString("Localization");
builder.Services.AddDbContext<LocalizationDbContext>(options => options.UseSqlServer(defaultConnString, b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

var app = builder.Build();

app.Run();

public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}