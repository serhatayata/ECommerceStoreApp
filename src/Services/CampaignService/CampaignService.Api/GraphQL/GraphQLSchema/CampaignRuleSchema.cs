using CampaignService.Api.GraphQL.GraphQLMutations;
using CampaignService.Api.GraphQL.GraphQLQueries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLSchema;

public class CampaignRuleSchema : Schema
{
    public CampaignRuleSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CampaignRuleQuery>();
        Mutation = provider.GetRequiredService<CampaignRuleMutation>();
    }
}
