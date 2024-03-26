using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer.Api.Attributes;
using IdentityServer.Api.DependencyResolvers.Autofac;

namespace IdentityServer.Api.Configurations.Installers.HostInstallers;

[InstallerOrder(Order = 10)]
public class DependencyResolverHostInstaller : IHostInstaller
{
    public Task Install(
        IHostBuilder host, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));

        return Task.CompletedTask;
    }
}
