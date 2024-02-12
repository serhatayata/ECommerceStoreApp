namespace CampaignService.Api.Models.Enums;

public enum CouponItemStatus : byte
{
    /// <summary>
    /// Active and usable
    /// </summary>
    Active = 0,

    /// <summary>
    /// Coupon is used
    /// </summary>
    Used = 1,

    /// <summary>
    /// Coupon item is passive and not usable
    /// </summary>
    Passive = 2
}
