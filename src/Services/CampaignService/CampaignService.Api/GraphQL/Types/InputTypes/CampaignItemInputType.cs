using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types.InputTypes;

public class CampaignItemInputType : InputObjectGraphType
{
    public CampaignItemInputType()
    {
        Name = "campaignItemInput";
        Field<IntGraphType>("id");
        Field<NonNullGraphType<IntGraphType>>("campaignId");
        Field<NonNullGraphType<StringGraphType>>("userId");
        Field<NonNullGraphType<StringGraphType>>("description");
        Field<NonNullGraphType<CampaignItemStatusEnumType>>("status");
        Field<NonNullGraphType<DateTimeGraphType>>("expirationDate");
    }
}
