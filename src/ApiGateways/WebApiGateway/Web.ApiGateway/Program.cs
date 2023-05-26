using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Handlers;
using Web.ApiGateway.Middlewares;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Services.AddControllers();
builder.Configuration.AddConfiguration(config);

#region HOST
builder.Host.AddHostExtensions(environment);
#endregion
#region AUTH
builder.Services.ConfigureAuth(configuration);
#endregion
#region HTTP
builder.Services.AddTransient<HttpClientDelegatingHandler>();

builder.Services.ConfigureHttpClients(configuration);
#endregion
#region OCELOT CONFIGURATIONS
builder.Services.AddOcelot().AddConsul();
#endregion
#region CORS
builder.Services.ConfigureCors();
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.ApiGateway v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

var ocelotConfig = new OcelotPipelineConfiguration
{
    AuthorizationMiddleware = GatewayAuthorizationMiddleware.Authorize
};

await app.UseOcelot(ocelotConfig);

app.MapControllers();

app.Run();
