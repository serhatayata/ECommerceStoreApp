using Autofac.Extensions.DependencyInjection;
using Autofac;
using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Extensions;
using CatalogService.Api.Utilities.IoC;
using Microsoft.EntityFrameworkCore;
using System;
using CatalogService.Api.DependencyResolvers.Autofac;
using CatalogService.Api.Mapping;
using CatalogService.Api.Infrastructure.Interceptors;
using System.Reflection;
using CatalogService.Api.Services.Grpc;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Services.Cache.Concrete;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var assembly = typeof(Program).Assembly.GetName().Name;
IWebHostEnvironment environment = builder.Environment;

var config = ConfigurationExtension.appConfig;
var serilogConfig = ConfigurationExtension.serilogConfig;

builder.Services.AddControllerSettings();

#region SERVICES

#region Startup DI
builder.Services.AddSingleton<IRedisService, RedisService>();
#endregion
#region Host
builder.Host.AddHostExtensions(environment);
#endregion
#region Http
builder.Services.AddHttpContextAccessor();
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGrpcService<GrpcBrandService>();
app.MapGrpcService<GrpcCategoryService>();
app.MapGrpcService<GrpcCommentService>();
app.MapGrpcService<GrpcFeatureService>();
app.MapGrpcService<GrpcProductService>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGrpcReflectionService();
}

app.Run();

public partial class Program
{
    public static string appName = Assembly.GetExecutingAssembly().GetName().Name;
}