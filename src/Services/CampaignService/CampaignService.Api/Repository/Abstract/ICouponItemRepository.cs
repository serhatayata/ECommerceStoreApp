using CampaignService.Api.Entities;
using System.Linq.Expressions;

namespace CampaignService.Api.Repository.Abstract;

public interface ICouponItemRepository
{
    /// <summary>
    /// Create coupon
    /// </summary>
    /// <param name="model">Model of the data added</param>
    /// <returns><see cref="bool"/> model</returns>
    Task<CouponItem?> CreateAsync(CouponItem model);

    /// <summary>
    /// Create bulk coupons
    /// </summary>
    /// <param name="model">Model of the data added</param>
    /// <returns><see cref="bool"/> model</returns>
    Task<bool> CreateBulkAsync(List<CouponItem> model);

    /// <summary>
    /// Update coupon, returns updated entity, if not updated, returns null
    /// </summary>
    /// <param name="model">Model of updated data</param>
    /// <returns></returns>
    Task<CouponItem?> UpdateAsync(CouponItem model);

    /// <summary>
    /// Delete coupon item by id
    /// </summary>
    /// <param name="id">Id of the campaign item</param>
    /// <returns><see cref="bool"/> value, TRUE if deleted</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Get campaign item by id
    /// </summary>
    /// <param name="id">Id of the campaign item</param>
    /// <returns><see cref="CouponItem"/></returns>
    Task<CouponItem?> GetAsync(int id);

    /// <summary>
    /// Get all campaign items
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CouponItem>> GetAllAsync();

    /// <summary>
    /// Get all campaign items
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CouponItem>> GetAllByFilterAsync(Expression<Func<CouponItem, bool>> expression);
}
