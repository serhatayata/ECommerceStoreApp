using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;
using CampaignService.Api.GraphQL.Types.Enums;
using CampaignService.Api.Repository.Abstract;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class CampaignType : ObjectGraphType<Campaign>
{
    public CampaignType()
    {
        Field(o => o.Id, type: typeof(IdGraphType)).Description("Id property from the campaign object");
        Field<CampaignStatusEnumType>("status").Resolve(context => context.Source.Status);
        Field(o => o.Name).Description("Name property from the campaign");
        Field(o => o.Description).Description("Description property from the campaign");
        Field(o => o.ExpirationDate).Description("ExpirationDate property from the campaign");
        Field(o => o.StartDate).Description("StartDate property from the campaign");
        Field(o => o.CreationDate).Description("CreationDate property from the campaign");
        Field(o => o.UpdateDate).Description("UpdateDate property from the campaign");
        Field(o => o.Sponsor).Description("Sponsor property from the campaign");
        Field<CampaignDiscountTypeEnumType>("discountType").Resolve(context => context.Source.DiscountType);
        Field<PlatformTypeEnumType>("platformType").Resolve(context => context.Source.PlatformType);
        Field<CalculationTypeEnumType>("calculationType").Resolve(context => context.Source.CalculationType);
        Field(o => o.CalculationAmount).Description("Calculation amount property from the campaign");
        Field(o => o.Amount).Description("Rate property from the campaign");
        Field(o => o.IsForAllCategory).Description("IsForAllCategory property from the campaign");
        Field(o => o.MaxUsage).Description("Max usage property from the campaign");
        Field(o => o.MaxUsagePerUser).Description("Max usage per user property from the campaign");
        Field<ListGraphType<CampaignSourceType>, IEnumerable<CampaignSource>>("campaignSources")
            .Description("All campaign sources for this campaign")
            .ResolveAsync(context =>
            {
                var loader = context.RequestServices?.GetRequiredService<CampaignSourceCollectionBatchDataLoader>();
                return loader?.LoadAsync(context.Source.Id);
            });
        Field<ListGraphType<CampaignItemType>, IEnumerable<CampaignItem>>("campaignItems")
            .Description("All campaign items for this campaign")
            .ResolveAsync(context =>
            {
                var loader = context.RequestServices?.GetRequiredService<CampaignItemCollectionBatchDataLoader>();
                return loader?.LoadAsync(context.Source.Id);
            });
        // CAMPAIGN RULES WILL BE ADDED HERE
    }
}
