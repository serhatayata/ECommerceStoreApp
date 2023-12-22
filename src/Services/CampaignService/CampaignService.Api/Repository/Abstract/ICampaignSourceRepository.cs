using CampaignService.Api.Entities;

namespace CampaignService.Api.Repository.Abstract;

public interface ICampaignSourceRepository
{
    /// <summary>
    /// Create campaign source
    /// </summary>
    /// <param name="model">Model of the data added</param>
    /// <returns><see cref="bool"/> value, TRUE if created</returns>
    Task<bool> Create(CampaignSource model);
    /// <summary>
    /// Update campaign source, returns updated entity
    /// </summary>
    /// <param name="model">Model of updated data</param>
    /// <returns></returns>
    Task<CampaignSource?> Update(CampaignSource model);
    /// <summary>
    /// Delete campaign source by id
    /// </summary>
    /// <param name="id">Id of the campaign source</param>
    /// <returns><see cref="bool"/> value, TRUE if deleted</returns>
    Task<bool> Delete(int id);
    /// <summary>
    /// Get campaign source by id
    /// </summary>
    /// <param name="id">Id of the campaign source</param>
    /// <returns><see cref="CampaignSource"/></returns>
    Task<CampaignSource> GetAsync(int id);
    /// <summary>
    /// Get all campaign sources
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CampaignSource>> GetAllAsync();
    /// <summary>
    /// Get all by campaign id
    /// </summary>
    /// <param name="id">Id of the campaign</param>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CampaignSource>> GetAllByCampaignIdAsync(int id);
}
