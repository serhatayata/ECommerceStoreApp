using OrderService.Api.Models.Settings;

namespace OrderService.Api.Configurations.Installers.ServiceInstallers;

public class StartupDIServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        //services.AddScoped<IClientCredentialsTokenService, ClientCredentialsTokenService>();
        //services.AddSingleton<IRedisService, RedisService>();
        //services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        //{
        //    var address = configuration["ConsulConfig:Address"];
        //    consulConfig.Address = new Uri(address);
        //}));

        return Task.CompletedTask;
    }
}
