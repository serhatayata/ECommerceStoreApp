using CampaignService.Api.GraphQL.GraphQLMutations;
using CampaignService.Api.GraphQL.GraphQLQueries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLSchema;

public class CampaignSourceSchema : Schema
{
    public CampaignSourceSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CampaignSourceQuery>();
        //Mutation = provider.GetRequiredService<CampaignSourceMutation>();
    }
}
