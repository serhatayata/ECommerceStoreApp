using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types.InputTypes;

public class CouponItemInputType : InputObjectGraphType
{
    public CouponItemInputType()
    {
        Name = "couponItemInput";
        Field<IntGraphType>("id");
        Field<NonNullGraphType<IntGraphType>>("couponId");
        Field<StringGraphType>("userId");
        Field<NonNullGraphType<CouponItemStatusEnumType>>("status");
        Field<IntGraphType>("orderId");
    }
}
