using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types.InputTypes;

public class CouponUsageInputType : InputObjectGraphType
{
    public CouponUsageInputType()
    {
        Name = "couponUsageInput";
        Field<NonNullGraphType<StringGraphType>>("code");
        Field<StringGraphType>("userId");
    }
}
