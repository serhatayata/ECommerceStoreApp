using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.GraphQL.Types.Inputs;

public class CampaignItemInput
{
    public int Id { get; set; }
    public int CampaignId { get; set; }
    public string UserId { get; set; }
    public string Description { get; set; }
    public CampaignItemStatus Status { get; set; }
    public DateTime ExpirationDate { get; set; }
}
