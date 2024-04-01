using MonitoringService.Api.Configurations.Installers;
using MonitoringService.Api.Extensions;
using MonitoringService.Api.Extensions.MiddlewareExtensions;

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

app.UseExceptionMiddleware();
#region Routing-Redirection
app.UseHttpsRedirection();
app.UseRouting();
#endregion
#region Auth
app.UseAuthentication();
app.UseAuthorization();
#endregion

app.InstallApplicationBuilder(app.Lifetime,
                              configuration,
                              typeof(IApplicationBuilderInstaller).Assembly);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Start();

app.InstallServiceDiscovery(app.Lifetime, configuration);

app.WaitForShutdown();