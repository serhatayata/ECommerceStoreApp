using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.Models.Coupon;

public class CouponUsage
{
    public CouponUsage(string reason)
    {
        this.Reason = reason;
    }

    public string Code { get; set; }

    public string? UserId { get; set; }

    public string Name { get; set; }

    public string? Reason { get; set; }

    public CouponTypes Type { get; set; }

    public UsageTypes UsageType { get; set; }

    public CalculationTypes CalculationType { get; set; }

    public decimal CalculationAmount { get; set; }

    public decimal Amount { get; set; }

    public int MaxUsage { get; set; }

    public DateTime ExpirationDate { get; set; }
}
