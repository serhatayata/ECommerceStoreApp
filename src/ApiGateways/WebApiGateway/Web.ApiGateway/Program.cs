using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Handlers;
using Web.ApiGateway.Middlewares;
using Web.ApiGateway.Models.LogModels;
using Web.ApiGateway.Services.ElasticSearch.Abstract;
using Web.ApiGateway.Services.ElasticSearch.Concrete;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Configuration.AddConfiguration(config);

builder.Host.AddHostExtensions(environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Startup DI
builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
#endregion
#region Logging
builder.Services.AddLogging();
#endregion
#region Http
builder.Services.AddHttpContextAccessor();
#endregion

builder.Services.AddOcelot().AddConsul();
builder.Services.ConfigureCors();

#region Handlers
builder.Services.AddTransient<HttpClientDelegatingHandler>();
#endregion
#region ElasticSearch
var serviceProvider = builder.Services.BuildServiceProvider();
var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

var elasticLogOptions = configuration.GetSection("ElasticSearchOptions").Get<ElasticSearchOptions>();
await elasticSearchService.CreateIndexAsync<LogDetail>(elasticLogOptions.LogIndex);
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.ApiGateway v1"));
}

app.ConfigureCustomExceptionMiddleware();
app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

//This will be changed
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");
});

var ocelotConfig = new OcelotPipelineConfiguration
{
    AuthorizationMiddleware = GatewayAuthorizationMiddleware.Authorize
};
await app.UseOcelot(ocelotConfig);

app.MapControllers();

app.Run();
