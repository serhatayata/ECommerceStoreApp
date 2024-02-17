using CampaignService.Api.GraphQL.Mutations;
using CampaignService.Api.GraphQL.Queries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Schemas;

public class CouponSchema : Schema
{
    public CouponSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CouponQuery>();
        Mutation = provider.GetRequiredService<CouponMutation>();
    }
}
