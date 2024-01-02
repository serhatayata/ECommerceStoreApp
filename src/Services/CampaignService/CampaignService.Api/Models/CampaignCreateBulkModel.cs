namespace CampaignService.Api.Models;

public class CampaignCreateBulkModel
{
    /// <summary>
    /// Count of campaign items which will be created
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Campaign Id of items
    /// </summary>
    public int CampaignId { get; set; }
}
