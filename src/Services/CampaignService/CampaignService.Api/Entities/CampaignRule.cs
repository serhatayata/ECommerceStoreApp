namespace CampaignService.Api.Entities;

public class CampaignRule
{
    /// <summary>
    /// Id of the rule
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Name of the rule
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Description of the rule
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Value of the rule
    /// </summary>
    public string Value { get; set; }
}
