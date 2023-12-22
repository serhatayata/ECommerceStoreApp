using CampaignService.Api.Entities;
using CampaignService.Api.Infrastructure.Contexts;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;

//BatchDataLoader
public class CampaignSourceBatchDataLoader : DataLoaderBase<int, CampaignSource>
{
    private readonly CampaignDbContext _context;

    public CampaignSourceBatchDataLoader(
        CampaignDbContext context)
    {
        _context = context;
    }

    protected override async Task FetchAsync(
        IEnumerable<DataLoaderPair<int, CampaignSource>> list,
        CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IDictionary<int, CampaignSource> data = await _context.CampaignSources
                                    .Where(campaignSource => ids.Contains(campaignSource.Id))
                                    .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (DataLoaderPair<int, CampaignSource> entry in list)
            entry.SetResult(data.TryGetValue(entry.Key, out var campaignSource) ? campaignSource : null);
    }
}
