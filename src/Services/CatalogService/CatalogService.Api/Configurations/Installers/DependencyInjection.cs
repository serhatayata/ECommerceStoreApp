﻿using CatalogService.Api.Attributes;
using System.Reflection;

namespace CatalogService.Api.Configurations.Installers;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment hostEnvironment,
        params Assembly[] assemblies)
    {
        IEnumerable<IServiceInstaller> serviceInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>()
            .OrderBy(ord =>
            {
                var att = ord.GetType()
                             .GetCustomAttributes(typeof(InstallerOrderAttribute), true)
                             .FirstOrDefault() as InstallerOrderAttribute;

                if (att == null)
                    return int.MaxValue;

                return att.Order;
            });

        foreach (IServiceInstaller serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(services, configuration, hostEnvironment);
        }

        return services;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }

    public static IHostBuilder InstallHost(
    this IHostBuilder host,
    IConfiguration configuration,
    IWebHostEnvironment hostEnvironment,
    params Assembly[] assemblies)
    {
        IEnumerable<IHostInstaller> hostInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IHostInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IHostInstaller>();

        foreach (IHostInstaller hostInstaller in hostInstallers)
        {
            hostInstaller.Install(host, configuration, hostEnvironment);
        }

        return host;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }

    public static WebApplication InstallWebApp(
    this WebApplication app,
    IHostApplicationLifetime appLifeTime,
    IConfiguration configuration,
    params Assembly[] assemblies)
    {
        IEnumerable<IWebApplicationInstaller> webAppInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IWebApplicationInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IWebApplicationInstaller>();

        foreach (IWebApplicationInstaller webAppIstaller in webAppInstallers)
        {
            webAppIstaller.Install(app, appLifeTime, configuration);
        }

        return app;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }

    public static IApplicationBuilder InstallApplicationBuilder(
    this IApplicationBuilder app,
    IHostApplicationLifetime appLifeTime,
    IConfiguration configuration,
    params Assembly[] assemblies)
    {
        IEnumerable<IApplicationBuilderInstaller> webAppInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IApplicationBuilderInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IApplicationBuilderInstaller>();

        foreach (IApplicationBuilderInstaller webAppIstaller in webAppInstallers)
        {
            webAppIstaller.Install(app, appLifeTime, configuration);
        }

        return app;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }

    public static ConfigureWebHostBuilder InstallWebAppBuilder(
    this ConfigureWebHostBuilder builder,
    IWebHostEnvironment hostEnvironment,
    IConfiguration configuration,
    params Assembly[] assemblies)
    {
        IEnumerable<IWebHostBuilderInstaller> builderInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IWebHostBuilderInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IWebHostBuilderInstaller>();

        foreach (IWebHostBuilderInstaller builderInstaller in builderInstallers)
        {
            builderInstaller.Install(builder, hostEnvironment, configuration);
        }

        return builder;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }
}
