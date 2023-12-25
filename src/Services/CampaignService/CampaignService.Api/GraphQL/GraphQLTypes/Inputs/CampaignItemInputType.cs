using CampaignService.Api.GraphQL.GraphQLTypes.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLTypes.Inputs;

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
