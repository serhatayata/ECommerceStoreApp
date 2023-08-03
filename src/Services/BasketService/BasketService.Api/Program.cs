using Autofac;
using Autofac.Extensions.DependencyInjection;
using BasketService.Api.DependencyResolvers.Autofac;
using BasketService.Api.Extensions;
using BasketService.Api.Infrastructure.Filters;
using BasketService.Api.Infrastructure.Interceptors;
using BasketService.Api.Models;
using BasketService.Api.Models.Settings;
using BasketService.Api.Repositories.Abstract;
using BasketService.Api.Repositories.Concrete;
using BasketService.Api.Services.ElasticSearch.Abstract;
using BasketService.Api.Services.ElasticSearch.Concrete;
using BasketService.Api.Services.Grpc;
using BasketService.Api.Services.Redis.Abstract;
using BasketService.Api.Services.Redis.Concrete;
using BasketService.Api.Services.Token.Abstract;
using BasketService.Api.Services.Token.Concrete;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Services.AddElasticSearchConfiguration();

#region Controllers
builder.Services.AddControllerSettings();
#endregion
#region Startup DI
builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddTransient<IClientCredentialsTokenService, ClientCredentialsTokenService>();

builder.Services.AddTransient<IBasketRepository, BasketRepository>();
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

IdentityModelEventSource.ShowPII = true;
#endregion
#region Configuration
builder.Configuration.AddConfiguration(config);

builder.Services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
builder.Services.Configure<LocalizationSettings>(configuration.GetSection("LocalizationSettings"));
#endregion
#region Http
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClients(configuration);
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
#region Swagger
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Basket GRPC Http1/Http2 Service",
        Version = "v1",
        Description = "The Basket Service API"
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            ClientCredentials = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{builder.Configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
                TokenUrl = new Uri($"{builder.Configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
                Scopes = new Dictionary<string, string>() { { "basket_readpermission", "basket_writepermission" } }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});
#endregion
#region EventBus
builder.Services.RegisterEventBus(configuration);
builder.Services.SubscribeEvents();
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
#region Localization
await builder.Services.AddLocalizationSettingsAsync(configuration);
await builder.Services.AddLocalizationDataAsync(configuration);
#endregion

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<GrpcBasketService>();
app.MapControllers();

if (environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.MapGrpcReflectionService();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.Start();

app.RegisterWithConsul(app.Lifetime, configuration);

app.WaitForShutdown();

public partial class Program
{
    public static string appName = Assembly.GetExecutingAssembly().GetName().Name;
}