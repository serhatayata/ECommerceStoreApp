using Autofac;
using Autofac.Extensions.DependencyInjection;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Data.SeedData;
using LocalizationService.Api.DependencyResolvers.Autofac;
using LocalizationService.Api.Extensions;
using LocalizationService.Api.Mapping;
using LocalizationService.Api.Models.LogModels;
using LocalizationService.Api.Services.ElasticSearch.Abstract;
using LocalizationService.Api.Services.ElasticSearch.Concrete;
using LocalizationService.Api.Utilities.Configuration;
using LocalizationService.Api.Utilities.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Services.AddControllerSettings();
builder.Services.AddElasticSearchConfiguration();

#region Startup DI
builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
#endregion
#region Host
builder.Host.AddHostExtensions(environment);
#endregion
#region Configuration
builder.Configuration.AddConfiguration(config);
#endregion
#region Logging
builder.Services.AddLogging();
#endregion
#region DbContext
string defaultConnString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LocalizationDbContext>(options => options.UseSqlServer(defaultConnString, b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);
#endregion
#region Authentication / Authorization
builder.Services.AddAuthenticationConfigurations(configuration);
builder.Services.AddAuthorizationConfigurations(configuration);

IdentityModelEventSource.ShowPII = true;
#endregion
#region Http
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#endregion
#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
#endregion
#region AutoMapper
builder.Services.AddAutoMapper(typeof(MapProfile).Assembly);
#endregion
#region ElasticSearch
var serviceProvider = builder.Services.BuildServiceProvider();
var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

var elasticLogOptions = configuration.GetSection("ElasticSearchOptions").Get<ElasticSearchOptions>();
await elasticSearchService.CreateIndexAsync<LogDetail>(elasticLogOptions.LogIndex);
#endregion
#region ServiceTool
ServiceTool.Create(builder.Services);
#endregion
#region SeedData
var localizationDbContext = scope.ServiceProvider.GetService<LocalizationDbContext>();

await LocalizationSeedData.LoadLocalizationSeedDataAsync(localizationDbContext, scope, environment, configuration);
#endregion
#region Consul
builder.Services.ConfigureConsul(configuration);
#endregion
#region StaticHelpers
SpecificConfigurationHelper.Initialize(configuration);
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();
app.UseStaticFiles();
//app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Start();

app.RegisterWithConsul(app.Lifetime, configuration);

app.WaitForShutdown();