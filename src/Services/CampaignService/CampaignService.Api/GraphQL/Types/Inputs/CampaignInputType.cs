using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types.Inputs;

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
        Field<NonNullGraphType<CampaignTypeEnumType>>("type");
        Field<NonNullGraphType<DecimalGraphType>>("rate");
        Field<NonNullGraphType<DecimalGraphType>>("amount");
        Field<NonNullGraphType<BooleanGraphType>>("isForAllCategory");
    }
}
