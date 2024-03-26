using IdentityServer.Api.Configurations.Installers;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Extensions.MiddlewareExtensions;

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

#region PIPELINE

app.ConfigureCustomExceptionMiddleware();
app.UseStaticFiles();
app.UseSession();
//app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.InstallWebApp(app.Lifetime,
                  configuration,
                  typeof(IWebApplicationInstaller).Assembly);

app.InstallApplicationBuilder(app.Lifetime,
                              configuration,
                              typeof(IApplicationBuilderInstaller).Assembly);

app.MapControllers();
#endregion

app.Start();

app.InstallServiceDiscovery(app.Lifetime, configuration);

app.WaitForShutdown();
