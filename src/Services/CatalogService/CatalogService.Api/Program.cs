using Autofac;
using Autofac.Extensions.DependencyInjection;
using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Data.SeedData;
using CatalogService.Api.DependencyResolvers.Autofac;
using CatalogService.Api.Extensions;
using CatalogService.Api.Extensions.Middlewares;
using CatalogService.Api.Infrastructure.Interceptors;
using CatalogService.Api.IntegrationEvents.EventHandling;
using CatalogService.Api.IntegrationEvents.Events;
using CatalogService.Api.Mapping;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Models.Settings;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Base.Concrete;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Services.Cache.Concrete;
using CatalogService.Api.Services.Elastic.Abstract;
using CatalogService.Api.Services.Elastic.Concrete;
using CatalogService.Api.Services.Grpc;
using CatalogService.Api.Services.Token.Abstract;
using CatalogService.Api.Services.Token.Concrete;
using CatalogService.Api.Utilities.IoC;
using CatalogService.Api.Utilities.Options;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using IntegrationEventLogEF;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Reflection;
using static IdentityModel.OidcConstants;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

#region SERVICES

#region Controller
builder.Services.AddControllerSettings();
#endregion
#region Log
builder.Services.AddLogConfiguration();
#endregion
#region Startup DI
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddTransient<IClientCredentialsTokenService, ClientCredentialsTokenService>();
#endregion
#region Host
builder.Host.AddHostExtensions(environment);
#endregion
#region Configuration
builder.Services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
#endregion
#region Http
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClients(configuration);
#endregion
#region ServiceTool
ServiceTool.Create(builder.Services);
#endregion
#region DbContext
string defaultConnString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    options.UseSqlServer(connectionString: defaultConnString,
                         sqlServerOptionsAction: sqlOptions =>
                         {
                             sqlOptions.MigrationsAssembly(assembly);
                             // If we enable retry on failure, then we have to use execution strategy to follow the transactions seperately
                             //sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                         });
}, ServiceLifetime.Scoped);

builder.Services.AddDbContext<IntegrationEventLogContext>(options =>
{
    options.UseSqlServer(connectionString: defaultConnString,
                     sqlServerOptionsAction: sqlOptions =>
                     {
                         sqlOptions.MigrationsAssembly(assembly);
                         //sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                     });
}, ServiceLifetime.Scoped);
#endregion
#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
#endregion
#region AutoMapper
builder.Services.AddAutoMapper(typeof(MapProfile).Assembly);
#endregion
#region AddGrpc
builder.Services.AddGrpc(g =>
{
    g.EnableDetailedErrors = true;
    g.Interceptors.Add<ExceptionInterceptor>();
});

builder.Services.AddGrpcReflection();

#region If we want to use gRPC for http1 request, we must enable AddJsonTranscoding to convert http request
//builder.Services.AddGrpc(g =>
//{
//    g.EnableDetailedErrors = true;
//    g.Interceptors.Add<ExceptionInterceptor>();
//}).AddJsonTranscoding();
#endregion
#endregion
#region Event Bus
builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = configuration.GetValue<int?>("EventBusConfigSettings:ConnectionRetryCount") ?? 5,
        EventNameSuffix = configuration.GetSection("EventBusConfigSettings:EventNameSuffix").Value ?? "IntegrationEvent",
        SubscriberClientAppName = configuration.GetSection("EventBusConfigSettings:SubscriberClientAppName").Value ?? "OrderService",
        EventBusType = EventBusType.RabbitMQ,
        Connection = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        }
        //Connection = new ConnectionFactory()
        //{
        //    HostName = configuration.GetSection("EventBusConfigSettings:HostName").Value ?? "c_rabbitmq"
        //}
    };

    return EventBusFactory.Create(config, sp);
});
#endregion
#region Localization
// DAHA SONRA DUZENLENECEK UZUN SURUYOR DIYE EKLENDI
if (environment.IsProduction())
{
    await builder.Services.AddLocalizationSettingsAsync(configuration);
    await builder.Services.AddLocalizationDataAsync(configuration);
}
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

#endregion

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<GrpcBrandService>();
app.MapGrpcService<GrpcCategoryService>();
app.MapGrpcService<GrpcCommentService>();
app.MapGrpcService<GrpcFeatureService>();
app.MapGrpcService<GrpcProductService>();

app.UseResponseTimeMiddleware();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });

    app.MapGrpcReflectionService();
}

#region Seed Data
var seedScope = app.Services.CreateScope();
var catalogDbContext = seedScope.ServiceProvider.GetService<CatalogDbContext>();

await CatalogSeedData.LoadSeedDataAsync(catalogDbContext, seedScope, environment, configuration);
#endregion
#region Event
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<ProductUpdatedIntegrationEvent, ProductUpdatedIntegrationEventHandler>();
#endregion

ConfigureEventBusForSubscription(app);

app.Run();

public partial class Program
{
    public static string appName = Assembly.GetExecutingAssembly().GetName().Name;

    private static void ConfigureEventBusForSubscription(WebApplication app)
    {
        var eventBus = app.Services.GetRequiredService<IEventBus>();

        //eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
    }
}

