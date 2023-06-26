using Autofac.Extensions.DependencyInjection;
using Autofac;
using BasketGrpcService.Extensions;
using BasketGrpcService.Models;
using BasketGrpcService.Services.ElasticSearch.Abstract;
using BasketGrpcService.Services.ElasticSearch.Concrete;
using BasketGrpcService.Services.Grpc;
using BasketGrpcService.DependencyResolvers.Autofac;
using BasketGrpcService.Interceptors;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
builder.Services.AddElasticSearchConfiguration();

#region Startup DI
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#endregion
#region Host
builder.Host.AddHostExtensions(environment);
#endregion
#region Logging
builder.Services.AddLogging();
#endregion
#region Authorization-Authentication
builder.Services.AddAuthenticationConfigurations(configuration);
builder.Services.AddAuthorizationConfigurations(configuration);
#endregion
#region Configuration
builder.Configuration.AddConfiguration(config);

builder.Services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
#endregion
#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
#endregion
#region ElasticSearch
var serviceProvider = builder.Services.BuildServiceProvider();
var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

var elasticLogOptions = configuration.GetSection("ElasticSearchOptions").Get<ElasticSearchOptions>();
await elasticSearchService.CreateIndexAsync<LogDetail>(elasticLogOptions.LogIndex);
#endregion
#region Consul
builder.Services.ConfigureConsul(configuration);
#endregion

builder.ConfigureGrpc();

builder.Services.AddGrpc(g =>
{
    g.Interceptors.Add<ExceptionInterceptor>();
}).AddJsonTranscoding();

builder.Services.AddGrpcReflection();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<GrpcBasketService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

if (environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Start();

app.RegisterWithConsul(app.Lifetime, configuration);

app.WaitForShutdown();