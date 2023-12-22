using CampaignService.Api.Entities;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLTypes;

public class CampaignSourceType : ObjectGraphType<CampaignSource>
{
    public CampaignSourceType()
    {
        Field(o => o.Id, type: typeof(IdGraphType)).Description("Id property from the campaign source object");
        Field(o => o.CampaignId).Description("Campaign Id property from the campaign source");
        Field(o => o.EntityId).Description("Entity Id property from the campaign source");
    }
}
