using CampaignService.Api.GraphQL.GraphQLMutations;
using CampaignService.Api.GraphQL.GraphQLQueries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLSchema;

public class CampaignItemSchema : Schema
{
    public CampaignItemSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CampaignItemQuery>();
        //Mutation = provider.GetRequiredService<CampaignItemMutation>();
    }
}
