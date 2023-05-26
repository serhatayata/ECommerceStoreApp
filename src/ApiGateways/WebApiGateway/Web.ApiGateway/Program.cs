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

builder.Configuration.AddConfiguration(config);

builder.Host.AddHostExtensions(environment);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddOcelot().AddConsul();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCors();

builder.Services.AddTransient<HttpClientDelegatingHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.ApiGateway v1"));
}

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
