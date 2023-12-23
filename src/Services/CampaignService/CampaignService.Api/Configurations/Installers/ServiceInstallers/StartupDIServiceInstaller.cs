
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;
using CampaignService.Api.GraphQL.GraphQLSchema;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Repository.Concrete;
using GraphQL.Types;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

public class StartupDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddScoped<ISchema, CampaignSchema>();
        services.AddScoped<ISchema, CampaignItemSchema>();
        services.AddScoped<ISchema, CampaignSourceSchema>();
        services.AddScoped<ISchema, CampaignRuleSchema>();

        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<ICampaignItemRepository, CampaignItemRepository>();
        services.AddScoped<ICampaignRuleRepository, CampaignRuleRepository>();
        services.AddScoped<ICampaignSourceRepository, CampaignSourceRepository>();

        //DataLoader
        services.AddScoped<CampaignBatchDataLoader>();
        services.AddScoped<CampaignItemBatchDataLoader>();
        services.AddScoped<CampaignSourceBatchDataLoader>();
        services.AddScoped<CampaignRuleBatchDataLoader>();
        services.AddScoped<CampaignSourceCollectionBatchDataLoader>();
        services.AddScoped<CampaignItemCollectionBatchDataLoader>();
    }
}
