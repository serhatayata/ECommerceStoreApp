using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;

public class CouponBatchDataLoader : DataLoaderBase<int, Coupon>
{
    private readonly CampaignDbContext _context;

    public CouponBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(
        IEnumerable<DataLoaderPair<int, Coupon>> list,
        CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IDictionary<int, Coupon> data = await _context.Coupons
                                    .Where(campaign => ids.Contains(campaign.Id))
                                    .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (DataLoaderPair<int, Coupon> entry in list)
            entry.SetResult(data.TryGetValue(entry.Key, out var campaign) ? campaign : null);
    }
}
