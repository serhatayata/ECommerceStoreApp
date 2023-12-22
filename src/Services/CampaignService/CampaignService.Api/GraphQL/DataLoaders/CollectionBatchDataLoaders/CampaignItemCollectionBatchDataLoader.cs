using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;

public class CampaignItemCollectionBatchDataLoader : DataLoaderBase<int, IEnumerable<CampaignItem>>
{
    private readonly CampaignDbContext _context;

    public CampaignItemCollectionBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(IEnumerable<DataLoaderPair<int, IEnumerable<CampaignItem>>> list, CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IEnumerable<CampaignItem> data = await _context.CampaignItems
                        .Where(source => ids.Contains(source.CampaignId))
                        .ToListAsync(cancellationToken);

        ILookup<int, CampaignItem> dataLookup = data.ToLookup(x => x.CampaignId);
        foreach (DataLoaderPair<int, IEnumerable<CampaignItem>> entry in list)
            entry.SetResult(dataLookup[entry.Key]);
    }
}
