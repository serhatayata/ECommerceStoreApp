using CampaignService.Api.GraphQL.Mutations;
using CampaignService.Api.GraphQL.Queries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Schemas;

public class CampaignItemSchema : Schema
{
    public CampaignItemSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CampaignItemQuery>();
        Mutation = provider.GetRequiredService<CampaignItemMutation>();
    }
}
