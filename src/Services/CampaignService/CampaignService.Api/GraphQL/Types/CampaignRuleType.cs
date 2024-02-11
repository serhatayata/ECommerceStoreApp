using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class CampaignRuleType : ObjectGraphType<CampaignRule>
{
    public CampaignRuleType()
    {
        Field(o => o.Id, type: typeof(IdGraphType)).Description("Id property from the campaign rule object");
        Field(o => o.CampaignId).Description("Campaign Id property from the campaign rule");
        Field<CampaignRuleTypeEnumType>("type").Resolve(context => context.Source.Type);
        Field(o => o.Data).Description("Data property from the campaign rule");
        Field(o => o.Value).Description("Data property from the campaign rule");
    }
}
