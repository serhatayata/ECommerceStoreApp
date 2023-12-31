using CampaignService.Api.GraphQL.Mutations;
using CampaignService.Api.GraphQL.Queries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Schemas;

public class CampaignSourceSchema : Schema
{
    public CampaignSourceSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CampaignSourceQuery>();
        Mutation = provider.GetRequiredService<CampaignSourceMutation>();
    }
}
