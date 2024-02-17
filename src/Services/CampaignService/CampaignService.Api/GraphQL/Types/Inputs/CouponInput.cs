using CampaignService.Api.Entities;
using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.GraphQL.Types.Inputs;

public class CouponInput
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public CouponTypes Type { get; set; }

    public UsageTypes UsageType { get; set; }

    public CalculationTypes CalculationType { get; set; }

    public decimal CalculationAmount { get; set; }

    public decimal Amount { get; set; }

    public int MaxUsage { get; set; }

    public int UsageCount { get; set; }

    public DateTime ExpirationDate { get; set; }

    public List<CouponItem> CouponItems { get; set; } = new List<CouponItem>();
}
