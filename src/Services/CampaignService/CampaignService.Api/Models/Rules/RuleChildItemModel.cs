namespace CampaignService.Api.Models.Rules;

public class RuleChildItemModel
{
    /// <summary>
    /// Entity name of the child item
    /// </summary>
    public string Entity { get; set; }
    /// <summary>
    /// Items for rule of entity
    /// </summary>
    public List<RuleItemModel> Items { get; set; }
    /// <summary>
    /// Child items for rule of entity
    /// </summary>
    public List<RuleChildItemModel> ChildItems { get; set; }
}
