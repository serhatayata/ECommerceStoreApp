using CampaignService.Api.Entities;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLTypes;

public class CampaignRuleType : ObjectGraphType<CampaignRule>
{
    public CampaignRuleType()
    {
        Field(o => o.Id, type: typeof(IdGraphType)).Description("Id property from the campaign rule object");
        Field(o => o.Name).Description("Name property from the campaign rule");
        Field(o => o.Description).Description("Description property from the campaign rule");
        Field(o => o.Value).Description("Value property from the campaign rule");
    }
}
