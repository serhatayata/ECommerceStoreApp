using CampaignService.Api.Models.Rules;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class RuleChildItemType : ObjectGraphType<RuleChildItemModel>
{
    public RuleChildItemType()
    {
        Field(o => o.Entity, type: typeof(StringGraphType)).Description("rule field name");
        Field(o => o.Items, type: typeof(ListGraphType<RuleItemType>)).Description("rule items type");
        Field(o => o.ChildItems, type: typeof(ListGraphType<RuleChildItemType>)).Description("rule child items type");
    }
}
