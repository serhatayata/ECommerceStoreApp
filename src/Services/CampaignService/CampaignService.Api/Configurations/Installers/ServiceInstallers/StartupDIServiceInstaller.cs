using CampaignService.Api.Attributes;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;
using CampaignService.Api.GraphQL.Schemas;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Repository.Concrete;
using CampaignService.Api.Services.Cache.Abstract;
using CampaignService.Api.Services.Cache.Concrete;
using CampaignService.Api.Services.Localization.Abstract;
using CampaignService.Api.Services.Localization.Concrete;
using GraphQL.Types;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 1)]
public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddScoped<ISchema, CampaignSchema>();
        services.AddScoped<ISchema, CampaignItemSchema>();
        services.AddScoped<ISchema, CampaignSourceSchema>();
        services.AddScoped<ISchema, CampaignRuleSchema>();
        services.AddScoped<ISchema, CouponSchema>();
        services.AddScoped<ISchema, CouponItemSchema>();

        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<ICampaignItemRepository, CampaignItemRepository>();
        services.AddScoped<ICampaignSourceRepository, CampaignSourceRepository>();
        services.AddScoped<ICampaignRuleRepository, CampaignRuleRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<ICouponItemRepository, CouponItemRepository>();

        //DataLoader
        services.AddScoped<CampaignBatchDataLoader>();
        services.AddScoped<CampaignItemBatchDataLoader>();
        services.AddScoped<CampaignSourceBatchDataLoader>();
        services.AddScoped<CampaignSourceBatchDataLoader>();
        services.AddScoped<CouponBatchDataLoader>();
        services.AddScoped<CouponItemBatchDataLoader>();
        services.AddScoped<CampaignRuleBatchDataLoader>();
        services.AddScoped<CampaignItemCollectionBatchDataLoader>();
        services.AddScoped<CouponItemCollectionBatchDataLoader>();

        services.AddSingleton<IRedisService, RedisService>();
        services.AddSingleton<ILocalizationService, LocalizationService>();
    }
}
