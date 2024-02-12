using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;

public class CampaignRuleBatchDataLoader : DataLoaderBase<int, CampaignRule>
{
    private readonly CampaignDbContext _context;

    public CampaignRuleBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(
        IEnumerable<DataLoaderPair<int, CampaignRule>> list,
        CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IDictionary<int, CampaignRule> data = await _context.CampaignRules
                                    .Where(campaignRule => ids.Contains(campaignRule.Id))
                                    .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (DataLoaderPair<int, CampaignRule> entry in list)
            entry.SetResult(data.TryGetValue(entry.Key, out var campaignRule) ? campaignRule : null);
    }
}
