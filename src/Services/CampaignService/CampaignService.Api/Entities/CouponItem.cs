namespace CampaignService.Api.Entities;

public class CouponItem
{
    /// <summary>
    /// Id of the item
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Id of the coupon
    /// </summary>
    public int CouponId { get; set; }

    /// <summary>
    /// Id of the user
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Code of the campaign item
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Id of the order
    /// </summary>
    public int OrderId { get; set; }
}
