using Localization.BackgroundTasks.Installers.ServiceInstallers;
using System.Reflection;

namespace Localization.BackgroundTasks.Installers;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices(
    this IServiceCollection services,
    IConfiguration configuration,
    IWebHostEnvironment hostEnvironment,
    params Assembly[] assemblies)
    {
        var servicePriorities = new List<Type>()
        {
            typeof(StartupDIServiceInstaller),
            typeof(HttpClientServiceInstaller)
        };

        IEnumerable<IServiceInstaller> serviceInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>()
            .Select(t => new
            {
                Value = t,
                Order = servicePriorities.IndexOf(t.GetType())
            })
            .OrderBy(ord =>
            {
                if (ord.Order != -1)
                    return false;
                return true;
            })
            .ThenBy(o => o.Order)
            .Select(s => s.Value)
            .ToList();

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
