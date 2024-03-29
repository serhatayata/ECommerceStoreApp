using FileService.Api.Configurations.Installers;
using FileService.Api.Extensions;

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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