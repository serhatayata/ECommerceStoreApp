using BasketService.Api.Models;
using BasketService.Api.Models.Settings;
using BasketService.Api.Repositories.Abstract;
using BasketService.Api.Repositories.Concrete;
using BasketService.Api.Services.ElasticSearch.Abstract;
using BasketService.Api.Services.ElasticSearch.Concrete;
using BasketService.Api.Services.Redis.Abstract;
using BasketService.Api.Services.Redis.Concrete;
using BasketService.Api.Services.Token.Abstract;
using BasketService.Api.Services.Token.Concrete;

namespace BasketService.Api.Configurations.Installers.ServiceInstallers;

public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
        services.AddSingleton<IRedisService, RedisService>();
        services.AddTransient<IClientCredentialsTokenService, ClientCredentialsTokenService>();

        services.AddTransient<IBasketRepository, BasketRepository>();

        services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
        services.Configure<LocalizationSettings>(configuration.GetSection("LocalizationSettings"));
    }
}
