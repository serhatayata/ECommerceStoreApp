using Autofac.Extensions.DependencyInjection;
using Autofac;
using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Extensions;
using CatalogService.Api.Utilities.IoC;
using Microsoft.EntityFrameworkCore;
using System;
using CatalogService.Api.DependencyResolvers.Autofac;
using CatalogService.Api.Mapping;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Services.AddControllerSettings();

#region SERVICES

#region Startup DI
//builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
#endregion
#region Host
builder.Host.AddHostExtensions(environment);
#endregion
#region ServiceTool
ServiceTool.Create(builder.Services);
#endregion
#region DbContext
string defaultConnString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(defaultConnString, b => b.MigrationsAssembly(assembly)), ServiceLifetime.Scoped);
#endregion
#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
#endregion
#region AutoMapper
builder.Services.AddAutoMapper(typeof(MapProfile).Assembly);
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
