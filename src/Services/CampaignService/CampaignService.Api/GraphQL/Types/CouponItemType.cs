using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class CouponItemType : ObjectGraphType<CouponItem>
{
    public CouponItemType()
    {
        Field(o => o.Id, type: typeof(IdGraphType)).Description("Id property from the coupon item object");
        Field(o => o.CouponId, type: typeof(NonNullGraphType<IntGraphType>)).Description("Coupon Id property from the coupon item");
        Field(o => o.UserId, type: typeof(IntGraphType)).Description("User Id property from the coupon item");
        Field<CouponItemStatusEnumType>("status").Resolve(context => context.Source.Status);
        Field(o => o.OrderId, type: typeof(IntGraphType)).Description("Order id property from the coupon item");
    }
}
