using CampaignService.Api.GraphQL.Mutations;
using CampaignService.Api.GraphQL.Queries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Schemas;

public class CampaignRuleSchema : Schema
{
    public CampaignRuleSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CampaignRuleQuery>();
        Mutation = provider.GetRequiredService<CampaignRuleMutation>();
    }
}
