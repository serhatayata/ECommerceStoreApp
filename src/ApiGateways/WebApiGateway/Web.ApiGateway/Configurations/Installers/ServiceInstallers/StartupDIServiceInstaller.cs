using Web.ApiGateway.Attributes;
using Web.ApiGateway.Handlers;
using Web.ApiGateway.Services.ElasticSearch.Abstract;
using Web.ApiGateway.Services.ElasticSearch.Concrete;

namespace Web.ApiGateway.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 1)]
public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
        services.AddTransient<HttpClientDelegatingHandler>();
        //services.AddSingleton<IRedisService, RedisService>();
        //services.AddSingleton<ILocalizationService, LocalizationService>();
    }
}