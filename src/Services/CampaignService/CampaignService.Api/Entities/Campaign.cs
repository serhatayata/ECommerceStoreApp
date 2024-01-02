using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.Entities;

public class Campaign
{
    /// <summary>
    /// Id of the campaign
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Status of the campaign
    /// </summary>
    public CampaignStatus Status { get; set; }
    /// <summary>
    /// Name of the campaign
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Description of the campaign
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Expiration date of the campaign
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    /// <summary>
    /// Start date of the campaign
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// Creation date of the campaign
    /// </summary>
    public DateTime CreationDate { get; set; }
    /// <summary>
    /// Update date of the campaign
    /// </summary>
    public DateTime UpdateDate { get; set; }
    /// <summary>
    /// Sponsor name of the campaign if exists
    /// </summary>
    public string Sponsor { get; set; }
    /// <summary>
    /// Type of the campaign
    /// </summary>
    public CampaignTypes CampaignType { get; set; }
    /// <summary>
    /// Rate of the campaign
    /// </summary>
    public decimal Rate { get; set; }
    /// <summary>
    /// Amount of the campaign
    /// </summary>
    public decimal Amount { get; set; }
    /// <summary>
    /// Whether if this campaign is for all categories specified in campaign entity's or not,
    /// If true then campaign entity's entity Ids are for categoryIds,
    /// If false then campaign entity's entity Ids are for productIds
    /// </summary>
    public bool IsForAllCategory { get; set; }
    /// <summary>
    /// Remaining count of the campaign
    /// </summary>
    public int MaxUsage { get; set; }
    /// <summary>
    /// Remaining count of the campaign
    /// </summary>
    public int UsageCount { get; set; }
    /// <summary>
    /// Sources of the campaign
    /// </summary>
    public virtual ICollection<CampaignSource> CampaignSources { get; set; }
    /// <summary>
    /// Items of the campaign
    /// </summary>
    public virtual ICollection<CampaignItem> CampaignItems { get; set; }
}
