using CampaignService.Api.Entities;
using CampaignService.Api.Models.Rules;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLTypes;

public class RuleModelType : ObjectGraphType<RuleModel>
{
    public RuleModelType()
    {
        Field(o => o.Conditions, type: typeof(ListGraphType<RuleConditionType>)).Description("Rule model conditions");
        Field(o => o.Operators, type: typeof(ListGraphType<RuleOperatorType>)).Description("Rule model operators");
        Field(o => o.Items, type: typeof(ListGraphType<RuleItemType>)).Description("Rule model items");
    }
}
