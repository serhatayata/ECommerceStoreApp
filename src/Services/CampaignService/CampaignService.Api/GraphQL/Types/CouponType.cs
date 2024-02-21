using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.DataLoaders.CollectionBatchDataLoaders;
using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class CouponType : ObjectGraphType<Coupon>
{
    public CouponType()
    {
        Field(o => o.Id, type: typeof(IdGraphType)).Description("Id property from the coupon object");
        Field(o => o.Name).Description("Name property from the coupon");
        Field(o => o.Description).Description("Description property from the coupon");
        Field<CouponTypeEnumType>("type").Resolve(context => context.Source.Type);
        Field<UsageTypeEnumType>("usageType").Resolve(context => context.Source.UsageType);
        Field<CalculationTypeEnumType>("calculationType").Resolve(context => context.Source.CalculationType);
        Field(o => o.CalculationAmount).Description("Calculation amount property from the coupon");
        Field(o => o.Amount).Description("Amount property from the coupon");
        Field(o => o.MaxUsage).Description("Max usage property from the coupon");
        Field(o => o.UsageCount).Description("Usage count property from the coupon");
        Field(o => o.Code, type: typeof(StringGraphType)).Description("Code property from the coupon");
        Field(o => o.ExpirationDate).Description("Expiration date property from the coupon");
        Field(o => o.CreationDate).Description("Creation date property from the coupon");
        Field<ListGraphType<CouponItemType>, IEnumerable<CouponItem>>("couponItems")
            .Description("All items for this coupon")
            .ResolveAsync(context =>
            {
                var loader = context.RequestServices?.GetRequiredService<CouponItemCollectionBatchDataLoader>();
                return loader?.LoadAsync(context.Source.Id);
            });
    }
}