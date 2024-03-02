using Ocelot.Middleware;
using Web.ApiGateway.Configurations.Installers;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Middlewares;

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
app.ConfigureCustomExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.ApiGateway v1"));
}

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

//This will be changed
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action}/{id?}");
//});

app.InstallWebApp(app.Lifetime,
                  configuration,
                  typeof(IWebApplicationInstaller).Assembly);

app.InstallApplicationBuilder(app.Lifetime,
                              configuration,
                              typeof(IApplicationBuilderInstaller).Assembly);

app.MapControllers();

var ocelotConfig = new OcelotPipelineConfiguration
{
    AuthorizationMiddleware = GatewayAuthorizationMiddleware.Authorize
};
await app.UseOcelot(ocelotConfig);

app.MapControllers();

app.Start();

app.InstallServiceDiscovery(app.Lifetime, configuration);

app.WaitForShutdown();