using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types.InputTypes;

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
