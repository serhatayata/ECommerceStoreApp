namespace CampaignService.Api.GraphQL.Types.Inputs;

public class CouponUsageInput
{
    public string Code { get; set; }

    public string? UserId { get; set; }
}