using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using MonitoringService.Api.DependencyResolvers.Autofac;
using MonitoringService.Api.Extensions;
using MonitoringService.Api.Extensions.MiddlewareExtensions;
using MonitoringService.Api.Infrastructure.Contexts;
using MonitoringService.Api.Mapping;
using MonitoringService.Api.Services.Cache.Abstract;
using MonitoringService.Api.Services.Cache.Concrete;
using MonitoringService.Api.Services.Token.Abstract;
using MonitoringService.Api.Services.Token.Concrete;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var appConfig = ConfigurationExtension.appConfig;

#region Controller
builder.Services.AddControllerSettings();
#endregion
#region Startup DI
builder.Services.AddScoped<IClientCredentialsTokenService, ClientCredentialsTokenService>();
builder.Services.AddSingleton<IRedisService, RedisService>();
#endregion
#region Log
builder.Services.AddLogConfiguration();
#endregion
#region Host
builder.Host.AddHostExtensions(environment);
#endregion
#region Authorization-Authentication
builder.Services.AddAuthenticationConfigurations(configuration);
builder.Services.AddAuthorizationConfigurations(configuration);

IdentityModelEventSource.ShowPII = true;
#endregion
#region Http
builder.Services.AddHttpClients(configuration);
#endregion
#region DbContext
var connString = configuration.GetConnectionString("MonitoringDB");
builder.Services.AddEntityFrameworkNpgsql()
                .AddDbContext<MonitoringDbContext>(options => 
                                                    options.UseNpgsql(connString)
                                                           .UseLowerCaseNamingConvention());
#endregion
#region Consul
builder.Services.ConfigureConsul(configuration);
#endregion
// DAHA SONRA DUZENLENECEK UZUN SURUYOR DIYE EKLENDI
//if (environment.IsProduction())
//{
    await builder.Services.AddLocalizationSettingsAsync(configuration);
    await builder.Services.AddLocalizationDataAsync(configuration);
//}
#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
#endregion
#region AutoMapper
builder.Services.AddAutoMapper(typeof(MapProfile).Assembly);
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionMiddleware();
#region Routing-Redirection
app.UseHttpsRedirection();
app.UseRouting();
#endregion
#region Auth
app.UseAuthentication();
app.UseAuthorization();
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Start();

app.RegisterWithConsul(app.Lifetime, configuration);

app.WaitForShutdown();