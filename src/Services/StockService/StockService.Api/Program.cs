using StockService.Api.Configurations.Installers;
using StockService.Api.Extensions;

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
                  typeof(IApplicationBuilderInstaller).Assembly);

app.MapControllers();

app.Start();

app.InstallServiceDiscovery(app.Lifetime, configuration);

app.WaitForShutdown();
