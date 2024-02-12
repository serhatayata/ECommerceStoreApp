using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.Entities;

public class CampaignRule : IEntity
{
    /// <summary>
    /// Id of the campaign rule
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Campaign Id of the rule
    /// </summary>
    public int CampaignId { get; set; }

    /// <summary>
    /// Campaign rule type
    /// </summary>
    public CampaignRuleTypes Type { get; set; }

    /// <summary>
    /// Campaign rule data
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Campaign rule value
    /// </summary>
    public string Value { get; set; }

    public virtual Campaign Campaign { get; set; }
}
