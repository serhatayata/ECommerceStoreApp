namespace CampaignService.Api.Models.Rules;

public class RuleItemModel
{
    public RuleItemModel()
    {
        
    }

    public RuleItemModel(string field, string type)
    {
        this.Field = field;
        this.Type = type;
    }

    /// <summary>
    /// Field of the entity
    /// </summary>
    public string Field { get; set; }
    /// <summary>
    /// Type of the field
    /// </summary>
    public string Type { get; set; }
}
