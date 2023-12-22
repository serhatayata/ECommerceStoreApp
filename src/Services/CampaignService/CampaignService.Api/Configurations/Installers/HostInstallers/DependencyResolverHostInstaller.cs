using Autofac;
using Autofac.Extensions.DependencyInjection;
using CampaignService.Api.DependencyResolvers.Autofac;

namespace CampaignService.Api.Configurations.Installers.HostInstallers;

public class DependencyResolverHostInstaller : IHostInstaller
{
    public void Install(IHostBuilder host, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
    }
}
