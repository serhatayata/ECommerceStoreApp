
using CampaignService.Api.GraphQL.GraphQLSchema;
using GraphQL;
using GraphQL.DataLoader;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

public class GraphQLServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
        services.AddSingleton<DataLoaderDocumentListener>();

        services.AddGraphQL(builder =>
                     builder
                    .AddSystemTextJson()
                    .AddNewtonsoftJson()
                    .AddSchema<CampaignItemSchema>()
                    .AddSchema<CampaignSourceSchema>()
                    .AddSchema<CampaignRuleSchema>()
                    .AddSchema<CampaignSchema>()
                    .AddGraphTypes(typeof(CampaignItemSchema).Assembly)
                    .AddGraphTypes(typeof(CampaignSourceSchema).Assembly)
                    .AddGraphTypes(typeof(CampaignRuleSchema).Assembly)
                    .AddGraphTypes(typeof(CampaignSchema).Assembly)
                    .AddDataLoader());

    }
}
