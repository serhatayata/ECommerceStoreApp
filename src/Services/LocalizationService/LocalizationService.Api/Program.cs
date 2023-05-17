using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using IdentityServer.Api.DependencyResolvers.Autofac;
using IdentityServer.Api.Mapping;
using LocalizationService.Api.Extensions;
using LocalizationService.Api.Models.LogModels;
using LocalizationService.Api.Services.ElasticSearch.Abstract;
using LocalizationService.Api.Services.ElasticSearch.Concrete;
using LocalizationService.Api.Utilities.IoC;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

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
#region Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});
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

builder.Services.AddControllers();
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
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
