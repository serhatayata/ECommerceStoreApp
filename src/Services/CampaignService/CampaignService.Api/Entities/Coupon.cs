﻿using CampaignService.Api.Models.Enums;

namespace CampaignService.Api.Entities;

public class Coupon
{
    /// <summary>
    /// Id of the coupon
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the coupon
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the coupon
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Type of the coupon
    /// </summary>
    public CouponTypes Type { get; set; }

    /// <summary>
    /// Usage type of the coupon
    /// </summary>
    public UsageTypes UsageType { get; set; }

    /// <summary>
    /// Amount of the coupon
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Total max item count of the coupon
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total max item count of the coupon
    /// </summary>
    public int UsageCount { get; set; }

    /// <summary>
    /// Code of the coupon
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Expiration date of the coupon
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Creation date of the coupon
    /// </summary>
    public DateTime CreationDate { get; set; }
}
