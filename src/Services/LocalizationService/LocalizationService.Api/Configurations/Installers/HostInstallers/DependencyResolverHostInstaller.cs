using Autofac;
using Autofac.Extensions.DependencyInjection;
using LocalizationService.Api.Attributes;
using LocalizationService.Api.DependencyResolvers.Autofac;

namespace LocalizationService.Api.Configurations.Installers.HostInstallers;

[InstallerOrder(Order = 2)]
public class DependencyResolverHostInstaller : IHostInstaller
{
    public Task Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));

        return Task.CompletedTask;
    }
}
