namespace CampaignService.Api.Models.Rules;

public class RuleModel
{
    /// <summary>
    /// Items for rule of entity
    /// </summary>
    public List<RuleItemModel> Items { get; set; } = new List<RuleItemModel>();

    /// <summary>
    /// Child items for rule of entity
    /// </summary>
    public List<RuleChildItemModel> ChildItems { get; set; } = new List<RuleChildItemModel>();

    /// <summary>
    /// Conditions for logic 
    /// </summary>
    public List<RuleConditionModel> Conditions { get; set; } = new List<RuleConditionModel>();

    /// <summary>
    /// Operators for logic 
    /// </summary>
    public List<RuleOperatorModel> Operators { get; set; } = new List<RuleOperatorModel>();
}
