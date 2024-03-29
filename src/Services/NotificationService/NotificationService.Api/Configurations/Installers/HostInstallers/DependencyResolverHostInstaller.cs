using Autofac;
using Autofac.Extensions.DependencyInjection;
using NotificationService.Api.DependencyResolvers.Autofac;

namespace NotificationService.Api.Configurations.Installers.HostInstallers;

public class DependencyResolverHostInstaller : IHostInstaller
{
    public Task Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModel()));

        return Task.CompletedTask;
    }
}