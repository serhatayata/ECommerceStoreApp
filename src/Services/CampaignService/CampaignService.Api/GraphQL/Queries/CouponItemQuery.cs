using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Services.Cache.Abstract;
using CampaignService.Api.Utilities;
using CampaignService.Api.Utilities.Json;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using System.Reflection;
using System.Text.Json; 

namespace CampaignService.Api.GraphQL.Queries;

public class CouponItemQuery : ObjectGraphType<CouponItem>
{
    private int cacheDbId = 11;
    private string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? string.Empty;

    public CouponItemQuery(
        ICouponItemRepository couponItemRepository,
        IRedisService redisService)
    {
        Name = nameof(CouponItemQuery);
        Description = $"{nameof(CouponItemQuery)} description";

        Field<CouponItemType, CouponItem>(name: "couponItem")
            .Description("coupon item type description")
            .Argument<IdGraphType>("id")
            .ResolveAsync(context =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for id"));
                    return null;
                }

                var loader = context.RequestServices.GetRequiredService<CouponItemBatchDataLoader>();
                return loader.LoadAsync(id);
            });

        Field<ListGraphType<CouponType>>(name: "allCouponItems")
            .Description("Coupon type description")
            .ResolveAsync(async (context) =>
            {
                var cacheKey = CacheExtensions.GetCacheKey("allCouponItems", className, null);
                return await redisService.GetAsync(cacheKey, cacheDbId,
                                                   new TimeSpan(2, 30, 0).Minutes,
                                                   async () =>
                                                   {
                                                       return await couponItemRepository.GetAllAsync();
                                                   });
            });

        Field<ListGraphType<CouponItemType>>(name: "couponItemsByFilter")
            .Description("Coupon item type description")
            .Argument<StringGraphType>("filter")
            .ResolveAsync(async (context) =>
            {
                var rule = context.GetArgument<string>("filter");
                if (string.IsNullOrWhiteSpace(rule))
                {
                    context.Errors.Add(new ExecutionError("Wrong value for id"));
                    return null;
                }

                var jsonDocument = JsonDocument.Parse(rule);
                var parser = new JsonExpressionParser();
                var expression = parser.ParseExpressionOf<CouponItem>(jsonDocument);
                return await couponItemRepository.GetAllByFilterAsync(expression);
            });

        Field<RuleModelType>(name: "getRuleModel")
            .Description("Get rule model")
            .Resolve(context =>
            {
                var cacheKey = CacheExtensions.GetCacheKey("getRuleModel", className, null);
                var result = redisService.Get(cacheKey, cacheDbId,
                                          new TimeSpan(2, 30, 0).Minutes,
                                          () => RuleModelBuilder.GetModelRule(typeof(CouponItem)));

                return result;
            });
    }
}
