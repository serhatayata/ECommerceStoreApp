using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types.InputTypes;

public class CouponInputType : InputObjectGraphType
{
    public CouponInputType()
    {
        Name = "couponInput";
        Field<IntGraphType>("id");
        Field<NonNullGraphType<StringGraphType>>("name");
        Field<NonNullGraphType<StringGraphType>>("description");
        Field<NonNullGraphType<CouponTypeEnumType>>("type");
        Field<NonNullGraphType<UsageTypeEnumType>>("usageType");
        Field<NonNullGraphType<CalculationTypeEnumType>>("calculationType");
        Field<NonNullGraphType<DecimalGraphType>>("calculationAmount");
        Field<NonNullGraphType<DecimalGraphType>>("amount");
        Field<NonNullGraphType<IntGraphType>>("maxUsage");
        Field<NonNullGraphType<DateTimeGraphType>>("expirationDate");
        Field<ListGraphType<CouponItemInputType>>("couponItems");
    }
}