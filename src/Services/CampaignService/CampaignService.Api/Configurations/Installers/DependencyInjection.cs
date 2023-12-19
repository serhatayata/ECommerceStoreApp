using System.Reflection;

namespace CampaignService.Api.Configurations.Installers;

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
            .Cast<IServiceInstaller>();

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
}
