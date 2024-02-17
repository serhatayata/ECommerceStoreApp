using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;

public class CouponItemCollectionBatchDataLoader : DataLoaderBase<int, IEnumerable<CouponItem>>
{
    private readonly CampaignDbContext _context;
    public CouponItemCollectionBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(IEnumerable<DataLoaderPair<int, IEnumerable<CouponItem>>> list, CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IEnumerable<CouponItem> data = await _context.CouponItems
                        .Where(source => ids.Contains(source.CouponId))
                        .ToListAsync(cancellationToken);

        ILookup<int, CouponItem> dataLookup = data.ToLookup(x => x.CouponId);
        foreach (DataLoaderPair<int, IEnumerable<CouponItem>> entry in list)
            entry.SetResult(dataLookup[entry.Key]);
    }
}
