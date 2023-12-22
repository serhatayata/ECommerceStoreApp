using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;

//BatchDataLoader
public class CampaignBatchDataLoader : DataLoaderBase<int, Campaign>
{
    private readonly CampaignDbContext _context;

    public CampaignBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(
        IEnumerable<DataLoaderPair<int, Campaign>> list,
        CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IDictionary<int, Campaign> data = await _context.Campaigns
                                    .Where(campaign => ids.Contains(campaign.Id))
                                    .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (DataLoaderPair<int, Campaign> entry in list)
            entry.SetResult(data.TryGetValue(entry.Key, out var campaign) ? campaign : null);
    }
}
