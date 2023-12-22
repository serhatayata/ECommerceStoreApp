using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.Entities;

public class CampaignItem
{
    /// <summary>
    /// Id of the item
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Id of the campaign
    /// </summary>
    public int CampaignId { get; set; }
    /// <summary>
    /// User Id of the campaign item
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// Description of the campaign item
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Code of the item
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// Status of the campaign item
    /// </summary>
    public CampaignItemStatus Status { get; set; }
    /// <summary>
    /// Creation date of the campaign item
    /// </summary>
    public DateTime CreationDate { get; set; }
    /// <summary>
    /// Expiration date of the campaign item
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    /// <summary>
    /// Item of the campaign
    /// </summary>
    public virtual Campaign Campaign { get; set; }
}
