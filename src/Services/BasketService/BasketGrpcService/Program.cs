using Autofac.Extensions.DependencyInjection;
using Autofac;
using BasketGrpcService.Extensions;
using BasketGrpcService.Models;
using BasketGrpcService.Services.ElasticSearch.Abstract;
using BasketGrpcService.Services.ElasticSearch.Concrete;
using BasketGrpcService.Services.Grpc;
using BasketGrpcService.DependencyResolvers.Autofac;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
builder.Services.AddElasticSearchConfiguration();

#region Configuration
builder.Configuration.AddConfiguration(config);
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

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GrpcBasketService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
