using CampaignService.Api.Entities;
using GraphQL.DataLoader;

namespace CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;

public class CouponItemCollectionBatchDataLoader : DataLoaderBase<int, IEnumerable<CouponItem>>
{
    public CouponItemCollectionBatchDataLoader()
    {
        
    }

    protected override Task FetchAsync(IEnumerable<DataLoaderPair<int, IEnumerable<CouponItem>>> list, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
