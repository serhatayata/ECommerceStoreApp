using CatalogService.Api.Attributes;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Models.Settings;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Base.Concrete;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Services.Cache.Concrete;
using CatalogService.Api.Services.Elastic.Abstract;
using CatalogService.Api.Services.Elastic.Concrete;
using CatalogService.Api.Services.Token.Abstract;
using CatalogService.Api.Services.Token.Concrete;
using CatalogService.Api.Utilities.IoC;

namespace CatalogService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 1)]
public class StartupDIServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IRedisService, RedisService>();
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddTransient<IClientCredentialsTokenService, ClientCredentialsTokenService>();

        services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDB"));

        ServiceTool.Create(services);

        return Task.CompletedTask;
    }
}
