using CampaignService.Api.Entities;
using CampaignService.Api.Models.CampaignRule;

namespace CampaignService.Api.Repository.Abstract;

public interface ICampaignRuleRepository
{
    /// <summary>
    /// Create campaignRule
    /// </summary>
    /// <param name="model">Model of the data added</param>
    /// <returns><see cref="bool"/> value, TRUE if created</returns>
    Task<CampaignRule?> CreateAsync(CampaignRule model);

    /// <summary>
    /// Update campaignRule, returns updated entity
    /// </summary>
    /// <param name="model">Model of updated data</param>
    /// <returns></returns>
    Task<CampaignRule?> UpdateAsync(CampaignRule model);

    /// <summary>
    /// Delete campaignRule by id
    /// </summary>
    /// <param name="id">Id of the campaignRule</param>
    /// <returns><see cref="bool"/> value, TRUE if deleted</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Get campaignRule by id
    /// </summary>
    /// <param name="id">Id of the campaignRule</param>
    /// <returns><see cref="CampaignRuleItem"/></returns>
    Task<CampaignRule?> GetAsync(int id);

    /// <summary>
    /// Get all campaign rules
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CampaignRule>> GetAllAsync();

    /// <summary>
    /// Get all campaign rules by filter
    /// </summary>
    /// <returns><see cref="List{T}"/></returns>
    Task<List<CampaignRule>> GetAllByFilterAsync(CampaignRuleGetByFilterModel model);
}
