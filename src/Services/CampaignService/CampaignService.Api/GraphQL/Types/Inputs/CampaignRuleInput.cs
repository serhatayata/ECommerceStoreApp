using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.GraphQL.Types.Inputs;

public class CampaignRuleInput
{
    public int CampaignId { get; set; }
    public CampaignRuleTypes Type { get; set; }
    public string Data { get; set; }
    public string Value { get; set; }
}
