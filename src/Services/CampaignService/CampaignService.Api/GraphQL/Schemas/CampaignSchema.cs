using CampaignService.Api.GraphQL.Mutations;
using CampaignService.Api.GraphQL.Queries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Schemas;

public class CampaignSchema : Schema
{
    public CampaignSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CampaignQuery>();
        Mutation = provider.GetRequiredService<CampaignMutation>();
    }
}
