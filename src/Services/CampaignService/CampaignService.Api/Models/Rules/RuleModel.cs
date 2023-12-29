namespace CampaignService.Api.Models.Rules;

public class RuleModel
{
    /// <summary>
    /// Conditions for logic 
    /// </summary>
    public List<RuleConditionModel> Conditions { get; set; }

    /// <summary>
    /// Operators for logic 
    /// </summary>
    public List<RuleOperatorModel> Operators { get; set; }

    /// <summary>
    /// Items for rule of entity
    /// </summary>
    public List<RuleItemModel> Items { get; set; }
}
