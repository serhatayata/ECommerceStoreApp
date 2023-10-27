namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        //services.AddSingleton<IRedisService, RedisService>();
        //services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        //{
        //    var address = configuration["ConsulConfig:Address"];
        //    consulConfig.Address = new Uri(address);
        //}));
    }
}
