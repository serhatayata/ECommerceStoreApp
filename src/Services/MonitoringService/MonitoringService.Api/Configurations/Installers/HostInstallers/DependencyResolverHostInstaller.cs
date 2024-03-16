using Autofac;
using Autofac.Extensions.DependencyInjection;
using MonitoringService.Api.Attributes;
using MonitoringService.Api.DependencyResolvers.Autofac;

namespace MonitoringService.Api.Configurations.Installers.HostInstallers;

[InstallerOrder(Order = 2)]
public class DependencyResolverHostInstaller : IHostInstaller
{
    public void Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
    }
}
