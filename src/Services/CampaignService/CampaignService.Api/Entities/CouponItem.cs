﻿using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.Entities;

public class CouponItem : IEntity
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
    /// Coupon item status
    /// </summary>
    public CouponItemStatus Status { get; set; }

    /// <summary>
    /// Id of the order
    /// </summary>
    public int? OrderId { get; set; }

    /// <summary>
    /// Coupon of the Item
    /// </summary>
    public virtual Coupon Coupon { get; set; }
}
