using CampaignService.Api.Entities;
using CampaignService.Api.Models.CampaignRule;
using CampaignService.Api.Models.Coupon;
using System.Linq.Expressions;

namespace CampaignService.Api.Repository.Abstract;

public interface ICouponRepository
{
    /// <summary>
    /// Create coupon
    /// </summary>
    /// <param name="model">Model of the data added</param>
    /// <returns><see cref="bool"/> value, TRUE if created</returns>
    Task<Coupon?> CreateAsync(Coupon model);

    /// <summary>
    /// Update coupon, returns updated entity
    /// </summary>
    /// <param name="model">Model of updated data</param>
    /// <returns></returns>
    Task<Coupon?> UpdateAsync(Coupon model);

    /// <summary>
    /// Delete coupon by id
    /// </summary>
    /// <param name="id">Id of the coupon</param>
    /// <returns><see cref="bool"/> value, TRUE if deleted</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Get coupon by id
    /// </summary>
    /// <param name="id">Id of the coupon</param>
    /// <returns><see cref="CouponItem"/></returns>
    Task<Coupon?> GetAsync(int id);

    /// <summary>
    /// Get all coupons
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<Coupon>> GetAllAsync();

    /// <summary>
    /// Get all coupons by filter
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<Coupon>> GetAllByFilterAsync(Expression<Func<Coupon, bool>> expression);
}
