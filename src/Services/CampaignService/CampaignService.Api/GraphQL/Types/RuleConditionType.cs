using CampaignService.Api.Models.Rules;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class RuleConditionType : ObjectGraphType<RuleConditionModel>
{
    public RuleConditionType()
    {
        Field(o => o.Name, type: typeof(StringGraphType)).Description("rule condition name");
        Field(o => o.Symbol, type: typeof(StringGraphType)).Description("rule condition symbol");
    }
}
