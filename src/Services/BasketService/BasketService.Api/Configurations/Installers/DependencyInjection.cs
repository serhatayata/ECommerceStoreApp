﻿using BasketService.Api.Configurations.Installers.ServiceInstallers;
using BasketService.Api.Configurations.Installers.WebApplicationInstallers;
using System.Reflection;

namespace BasketService.Api.Configurations.Installers;

public static class DependencyInjection
{
    public async static Task<IServiceCollection> InstallServices(
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
                if (ord.GetType() == typeof(StartupDIServiceInstaller))
                    return false;
                return true;
            });

        foreach (IServiceInstaller serviceInstaller in serviceInstallers)
        {
            await serviceInstaller.Install(services, configuration, hostEnvironment);
        }

        return services;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }

    public async static Task<IHostBuilder> InstallHost(
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
            await hostInstaller.Install(host, configuration, hostEnvironment);
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
        IEnumerable<IWebAppInstaller> webAppInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IWebAppInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IWebAppInstaller>()
            .Where(s => s.GetType() != typeof(ServiceDiscoveryWebAppInstaller));

        foreach (IWebAppInstaller webAppIstaller in webAppInstallers)
        {
            webAppIstaller.Install(app, appLifeTime, configuration);
        }

        return app;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }

    public async static Task<ConfigureWebHostBuilder> InstallWebHostBuilder(
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
            await builderInstaller.Install(builder, hostEnvironment, configuration);
        }

        return builder;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }
}
