using CampaignService.Api.GraphQL.GraphQLMutations;
using CampaignService.Api.GraphQL.GraphQLQueries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLSchema;

public class CampaignSchema : Schema
{
    public CampaignSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CampaignQuery>();
        //Mutation = provider.GetRequiredService<CampaignMutation>();
    }
}
