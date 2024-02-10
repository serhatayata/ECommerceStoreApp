using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types.InputTypes;

public class CampaignInputType : InputObjectGraphType
{
    public CampaignInputType()
    {
        Name = "campaignInput";
        Field<IntGraphType>("id");
        Field<NonNullGraphType<CampaignStatusEnumType>>("status");
        Field<NonNullGraphType<StringGraphType>>("name");
        Field<NonNullGraphType<StringGraphType>>("description");
        Field<NonNullGraphType<DateTimeGraphType>>("expirationDate");
        Field<NonNullGraphType<DateTimeGraphType>>("startDate");
        Field<NonNullGraphType<StringGraphType>>("sponsor");
        Field<NonNullGraphType<CampaignDiscountTypeEnumType>>("discountType");
        Field<NonNullGraphType<PlatformTypeEnumType>>("platformType");
        Field<NonNullGraphType<CalculationTypeEnumType>>("calculationType");
        Field<NonNullGraphType<DecimalGraphType>>("calculationAmount");
        Field<NonNullGraphType<DecimalGraphType>>("amount");
        Field<NonNullGraphType<BooleanGraphType>>("isForAllCategory");
        Field<NonNullGraphType<IntGraphType>>("maxUsage");
        Field<NonNullGraphType<IntGraphType>>("maxUsagePerUser");
    }
}