using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.GraphQL.Types.Inputs;

public class CouponItemInput
{
    public int Id { get; set; }

    public int CouponId { get; set; }

    public string? UserId { get; set; }

    public CouponItemStatus Status { get; set; }

    public int? OrderId { get; set; }
}
