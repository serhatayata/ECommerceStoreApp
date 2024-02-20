using CampaignService.Api.GraphQL.Mutations;
using CampaignService.Api.GraphQL.Queries;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Schemas;

public class CouponItemSchema : Schema
{
    public CouponItemSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<CouponItemQuery>();
        Mutation = provider.GetRequiredService<CouponItemMutation>();
    }
}
