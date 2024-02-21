using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.Types.Enums;
using CampaignService.Api.Models.Coupon;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class CouponUsageType : ObjectGraphType<CouponUsage>
{
    public CouponUsageType()
    {
        Field(o => o.Code, type: typeof(NonNullGraphType<StringGraphType>)).Description("Code property from the coupon");
        Field(o => o.UserId, type: typeof(StringGraphType)).Description("User id from the coupon");
        Field(o => o.Reason, type: typeof(StringGraphType)).Description("Reason property from the coupon");
        Field(o => o.Name).Description("Name property from the coupon");
        Field<CouponTypeEnumType>("type").Resolve(context => context.Source.Type);
        Field<UsageTypeEnumType>("usageType").Resolve(context => context.Source.UsageType);
        Field<CalculationTypeEnumType>("calculationType").Resolve(context => context.Source.CalculationType);
        Field(o => o.CalculationAmount).Description("Calculation amount property from the coupon");
        Field(o => o.Amount).Description("Amount property from the coupon");
        Field(o => o.MaxUsage).Description("Max usage property from the coupon");
        Field(o => o.ExpirationDate).Description("Expiration date property from the coupon");
    }
}
