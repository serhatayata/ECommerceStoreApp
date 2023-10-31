using Consul;
using NotificationService.Api.Models.Settings;
using NotificationService.Api.Services.Cache.Abstract;
using NotificationService.Api.Services.Cache.Concrete;
using NotificationService.Api.Services.Token.Abstract;
using NotificationService.Api.Services.Token.Concrete;

namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

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
