using Autofac.Extensions.DependencyInjection;
using Autofac;
using OrderService.Api.DependencyResolvers.Autofac;

namespace OrderService.Api.Configurations.Installers.HostInstallers;

public class DependencyResolverHostInstaller : IHostInstaller
{
    public void Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
    }
}
