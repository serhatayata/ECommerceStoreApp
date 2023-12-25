using CampaignService.Api.Entities;

namespace CampaignService.Api.Repository.Abstract;

public interface ICampaignRuleRepository
{
    /// <summary>
    /// Create campaign rule
    /// </summary>
    /// <param name="model">Model of the data added</param>
    /// <returns><see cref="bool"/> value, TRUE if created</returns>
    Task<CampaignRule?> CreateAsync(CampaignRule model);
    /// <summary>
    /// Update campaign rule, returns updated entity
    /// </summary>
    /// <param name="model">Model of updated data</param>
    /// <returns></returns>
    Task<CampaignRule?> UpdateAsync(CampaignRule model);
    /// <summary>
    /// Delete campaign rule by id
    /// </summary>
    /// <param name="id">Id of the campaign rule</param>
    /// <returns><see cref="bool"/> value, TRUE if deleted</returns>
    Task<bool> DeleteAsync(int id);
    /// <summary>
    /// Get campaign rule by id
    /// </summary>
    /// <param name="id">Id of the campaign rule</param>
    /// <returns><see cref="CampaignRule"/></returns>
    Task<CampaignRule?> GetAsync(int id);
    /// <summary>
    /// Get all campaign rules
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CampaignRule>> GetAllAsync();
}
