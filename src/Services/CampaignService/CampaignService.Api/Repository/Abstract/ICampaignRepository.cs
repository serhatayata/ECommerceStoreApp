using CampaignService.Api.Entities;

namespace CampaignService.Api.Repository.Abstract;

public interface ICampaignRepository
{
    /// <summary>
    /// Create campaign
    /// </summary>
    /// <param name="model">Model of the data added</param>
    /// <returns><see cref="bool"/> value, TRUE if created</returns>
    Task<Campaign?> CreateAsync(Campaign model);
    /// <summary>
    /// Update campaign, returns updated entity
    /// </summary>
    /// <param name="model">Model of updated data</param>
    /// <returns></returns>
    Task<Campaign?> UpdateAsync(Campaign model);
    /// <summary>
    /// Delete campaign by id
    /// </summary>
    /// <param name="id">Id of the campaign</param>
    /// <returns><see cref="bool"/> value, TRUE if deleted</returns>
    Task<bool> DeleteAsync(int id);
    /// <summary>
    /// Get campaign by id
    /// </summary>
    /// <param name="id">Id of the campaign</param>
    /// <returns><see cref="CampaignItem"/></returns>
    Task<Campaign?> GetAsync(int id);
    /// <summary>
    /// Get all campaigns
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<Campaign>> GetAllAsync();
}
