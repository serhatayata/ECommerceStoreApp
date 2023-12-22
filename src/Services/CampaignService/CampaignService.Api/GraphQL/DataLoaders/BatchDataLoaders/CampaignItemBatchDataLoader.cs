using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;

//BatchDataLoader
public class CampaignItemBatchDataLoader : DataLoaderBase<int, CampaignItem>
{
    private readonly CampaignDbContext _context;

    public CampaignItemBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(
        IEnumerable<DataLoaderPair<int, CampaignItem>> list,
        CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IDictionary<int, CampaignItem> data = await _context.CampaignItems
                                    .Where(campaignItem => ids.Contains(campaignItem.Id))
                                    .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (DataLoaderPair<int, CampaignItem> entry in list)
            entry.SetResult(data.TryGetValue(entry.Key, out var campaignItem) ? campaignItem : null);
    }
}
