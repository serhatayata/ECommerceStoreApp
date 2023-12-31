using CampaignService.Api.Models.Rules;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class RuleItemType : ObjectGraphType<RuleItemModel>
{
    public RuleItemType()
    {
        Field(o => o.Field, type: typeof(StringGraphType)).Description("rule field name");
        Field(o => o.Type, type: typeof(StringGraphType)).Description("rule type symbol");
    }
}
