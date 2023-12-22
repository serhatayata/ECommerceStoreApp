using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;
using CampaignService.Api.GraphQL.GraphQLTypes;
using CampaignService.Api.Repository.Abstract;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLQueries;

public class CampaignItemQuery : ObjectGraphType<CampaignItem>
{
    public CampaignItemQuery(
        ICampaignItemRepository repository)
    {
        Name = nameof(CampaignItemQuery);
        Description = $"{nameof(CampaignItemQuery)} description";

        Field<ListGraphType<CampaignItemType>, IEnumerable<CampaignItem>>(name: "itemsByCampaignId")
            .Description("campaign item type description")
            .ResolveAsync(context =>
            {
                var loader = context.RequestServices?.GetRequiredService<CampaignItemBatchDataLoader>();
                return loader?.LoadAsync(context.Source.Id);
            });
    }
}
