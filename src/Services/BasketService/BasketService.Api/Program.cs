using BasketService.Api.Configurations.Installers;
using BasketService.Api.Extensions;
using BasketService.Api.Services.Grpc;
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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.InstallWebApp(app.Lifetime,
                  configuration,
                  typeof(IWebAppInstaller).Assembly);

app.MapGrpcService<GrpcBasketService>();

app.MapControllers();

if (environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.MapGrpcReflectionService();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.InstallWebApp(app.Lifetime,
                  configuration,
                  typeof(IWebAppInstaller).Assembly);

app.Start();

app.InstallServiceDiscovery(app.Lifetime, configuration);

app.WaitForShutdown();

public partial class Program
{
    public static string appName = Assembly.GetExecutingAssembly().GetName().Name;
}