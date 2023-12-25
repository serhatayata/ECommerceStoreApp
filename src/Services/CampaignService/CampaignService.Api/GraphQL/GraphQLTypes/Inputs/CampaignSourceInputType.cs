using CampaignService.Api.GraphQL.GraphQLTypes.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLTypes.Inputs;

public class CampaignSourceInputType : InputObjectGraphType
{
    public CampaignSourceInputType()
    {
        Name = "campaignSourceInput";
        Field<IntGraphType>("id");
        Field<NonNullGraphType<IntGraphType>>("campaignId");
        Field<NonNullGraphType<IntGraphType>>("entityId");
    }
}
