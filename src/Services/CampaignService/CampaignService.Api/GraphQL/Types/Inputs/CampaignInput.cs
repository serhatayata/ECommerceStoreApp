using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.GraphQL.Types.Inputs;

public class CampaignInput
{
    public int Id { get; set; }
    public CampaignStatus Status { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime StartDate { get; set; }
    public string Sponsor { get; set; }
    public CampaignDiscountTypes DiscountType { get; set; }
    public CalculationTypes CalculationType { get; set; }
    public decimal CalculationAmount { get; set; }
    public decimal Amount { get; set; }
    public bool IsForAllCategory { get; set; }
    public int MaxUsage { get; set; }
    public int MaxUsagePerUser { get; set; }
}
