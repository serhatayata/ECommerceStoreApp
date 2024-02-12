using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.Models.CouponItem;

public class CouponItemGetByFilterModel
{
    /// <summary>
    /// Id of the coupon
    /// </summary>
    public int? CouponId { get; set; }

    /// <summary>
    /// Id of the user
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Code of the campaign item
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Coupon item status
    /// </summary>
    public CouponItemStatus? Status { get; set; }

    /// <summary>
    /// Id of the order
    /// </summary>
    public int? OrderId { get; set; }
}
