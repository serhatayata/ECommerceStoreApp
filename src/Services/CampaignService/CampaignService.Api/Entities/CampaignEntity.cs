namespace CampaignService.Api.Entities;

public class CampaignEntity
{
    /// <summary>
    /// Id of the campaign entity
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Campaign Id for entity
    /// </summary>
    public int CampaignId { get; set; }
    /// <summary>
    /// Entity Id for campaign entity
    /// For example, if campaign IsForAllCategory true and Entity Id is 5,
    /// then CategoryId 5 is specified here
    /// If campaign IsForAllCategory false and Entity Id is 6
    /// then ProductId 6 is specified here
    /// </summary>
    public int EntityId { get; set; }
}
