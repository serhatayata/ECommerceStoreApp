using CampaignService.Api.Entities;
using System.Linq.Expressions;

namespace CampaignService.Api.Repository.Abstract;

public interface ICampaignItemRepository
{
    /// <summary>
    /// Create campaign item
    /// </summary>
    /// <param name="model">Model of the data added</param>
    /// <returns><see cref="bool"/> value, TRUE if created</returns>
    Task<CampaignItem?> CreateAsync(CampaignItem model);
    /// <summary>
    /// Update campaign item, returns updated entity
    /// </summary>
    /// <param name="model">Model of updated data</param>
    /// <returns></returns>
    Task<CampaignItem?> UpdateAsync(CampaignItem model);
    /// <summary>
    /// Delete campaign item by id
    /// </summary>
    /// <param name="id">Id of the campaign item</param>
    /// <returns><see cref="bool"/> value, TRUE if deleted</returns>
    Task<bool> DeleteAsync(int id);
    /// <summary>
    /// Get campaign item by id
    /// </summary>
    /// <param name="id">Id of the campaign item</param>
    /// <returns><see cref="CampaignItem"/></returns>
    Task<CampaignItem?> GetAsync(int id);
    /// <summary>
    /// Get by expression
    /// </summary>
    /// <param name="predicate">expression</param>
    /// <returns><see cref="CampaignItem"/></returns>
    Task<CampaignItem?> GetAsync(Expression<Func<CampaignItem, bool>> predicate);
    /// <summary>
    /// Get all campaign items
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CampaignItem>> GetAllAsync();
    /// <summary>
    /// Get all by campaign id
    /// </summary>
    /// <param name="id">Id of the campaign</param>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CampaignItem>> GetAllByCampaignIdAsync(int id);
    /// <summary>
    /// Get all by rule
    /// </summary>
    /// <param name="rule">rule of campaign items</param>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CampaignItem>> GetAllByRulesync(string rule);
}
