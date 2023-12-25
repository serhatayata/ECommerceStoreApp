using CampaignService.Api.GraphQL.GraphQLTypes.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLTypes.Inputs;

public class CampaignRuleInputType : InputObjectGraphType
{
    public CampaignRuleInputType()
    {
        Name = "campaignRuleInput";
        Field<IntGraphType>("id");
        Field<NonNullGraphType<StringGraphType>>("name");
        Field<NonNullGraphType<StringGraphType>>("description");
        Field<NonNullGraphType<StringGraphType>>("value");
    }
}
