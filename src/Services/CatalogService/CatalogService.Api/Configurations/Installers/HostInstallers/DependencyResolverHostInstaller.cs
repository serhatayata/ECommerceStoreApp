using Autofac;
using Autofac.Extensions.DependencyInjection;
using CatalogService.Api.DependencyResolvers.Autofac;

namespace CatalogService.Api.Configurations.Installers.HostInstallers;

public class DependencyResolverHostInstaller : IHostInstaller
{
    public Task Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));

        return Task.CompletedTask;
    }
}
