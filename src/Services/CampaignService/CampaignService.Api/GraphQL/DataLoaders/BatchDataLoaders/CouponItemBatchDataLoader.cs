using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;

public class CouponItemBatchDataLoader : DataLoaderBase<int, CouponItem>
{
    private readonly CampaignDbContext _context;

    public CouponItemBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(
        IEnumerable<DataLoaderPair<int, CouponItem>> list,
        CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IDictionary<int, CouponItem> data = await _context.CouponItems
                                    .Where(couponItem => ids.Contains(couponItem.Id))
                                    .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (DataLoaderPair<int, CouponItem> entry in list)
            entry.SetResult(data.TryGetValue(entry.Key, out var couponItem) ? couponItem : null);
    }
}
