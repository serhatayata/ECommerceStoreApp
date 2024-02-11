using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.Models.CampaignRule;

public class CampaignRuleGetByFilterModel
{
    /// <summary>
    /// Campaign rule, campaign id filter
    /// </summary>
    public int? CampaignId { get; set; }

    /// <summary>
    /// Campaign rule type filter
    /// </summary>
    public CampaignRuleTypes? Type { get; set; }
}
