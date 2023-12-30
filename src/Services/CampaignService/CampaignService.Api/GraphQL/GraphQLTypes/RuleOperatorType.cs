using CampaignService.Api.Models.Rules;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLTypes;

public class RuleOperatorType : ObjectGraphType<RuleOperatorModel>
{
    public RuleOperatorType()
    {
        Field(o => o.Name, type: typeof(StringGraphType)).Description("rule operator name");
        Field(o => o.Symbol, type: typeof(StringGraphType)).Description("rule operator symbol");
    }
}
