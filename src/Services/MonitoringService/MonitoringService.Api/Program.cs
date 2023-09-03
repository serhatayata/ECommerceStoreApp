using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MonitoringService.Api.DependencyResolvers.Autofac;
using MonitoringService.Api.Extensions;
using MonitoringService.Api.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var appConfig = ConfigurationExtension.appConfig;

builder.Services.AddControllers();
#region Configuration
builder.Configuration.AddConfiguration(appConfig);
#endregion
#region Http
builder.Services.AddHttpClients(configuration);
#endregion
#region Host
builder.Host.AddHostExtensions(environment);
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
#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Start();

app.RegisterWithConsul(app.Lifetime, configuration);

app.WaitForShutdown();