using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types.InputTypes;

public class CampaignRuleInputType : InputObjectGraphType
{
    public CampaignRuleInputType()
    {
        Name = "campaignRuleInput";
        Field<NonNullGraphType<IntGraphType>>("id");
        Field<NonNullGraphType<IntGraphType>>("campaignId");
        Field<NonNullGraphType<CampaignRuleTypeEnumType>>("type");
        Field<NonNullGraphType<StringGraphType>>("data");
        Field<NonNullGraphType<StringGraphType>>("value");
    }
}
