using Consul;
using MonitoringService.Api.Services.Cache.Abstract;
using MonitoringService.Api.Services.Cache.Concrete;
using MonitoringService.Api.Services.Token.Abstract;
using MonitoringService.Api.Services.Token.Concrete;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddScoped<IClientCredentialsTokenService, ClientCredentialsTokenService>();
        services.AddSingleton<IRedisService, RedisService>();
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["ConsulConfig:Address"];
            consulConfig.Address = new Uri(address);
        }));
    }
}
