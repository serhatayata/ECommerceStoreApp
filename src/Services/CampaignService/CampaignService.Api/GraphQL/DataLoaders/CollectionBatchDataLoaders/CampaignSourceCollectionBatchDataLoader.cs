using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;

public class CampaignSourceCollectionBatchDataLoader : DataLoaderBase<int, IEnumerable<CampaignSource>>
{
    private readonly CampaignDbContext _context;

    public CampaignSourceCollectionBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(IEnumerable<DataLoaderPair<int, IEnumerable<CampaignSource>>> list, CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IEnumerable<CampaignSource> data = await _context.CampaignSources
                        .Where(source => ids.Contains(source.CampaignId))
                        .ToListAsync(cancellationToken);

        ILookup<int, CampaignSource> dataLookup = data.ToLookup(x => x.CampaignId);
        foreach (DataLoaderPair<int, IEnumerable<CampaignSource>> entry in list)
            entry.SetResult(dataLookup[entry.Key]);
    }
}
