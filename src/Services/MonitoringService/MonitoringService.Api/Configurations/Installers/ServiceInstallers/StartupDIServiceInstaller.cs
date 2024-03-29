using Consul;
using MonitoringService.Api.Services.Cache.Abstract;
using MonitoringService.Api.Services.Cache.Concrete;
using MonitoringService.Api.Services.Token.Abstract;
using MonitoringService.Api.Services.Token.Concrete;
using MonitoringService.Api.Attributes;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 1)]
public class StartupDIServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddScoped<IClientCredentialsTokenService, ClientCredentialsTokenService>();
        services.AddSingleton<IRedisService, RedisService>();
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["ConsulConfig:Address"];
            consulConfig.Address = new Uri(address);
        }));

        return Task.CompletedTask;
    }
}
