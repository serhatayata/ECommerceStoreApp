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
using System.Data;
using System.Reflection;
using System.Text.Json;

namespace CampaignService.Api.GraphQL.Queries;

public class CouponQuery : ObjectGraphType<Coupon>
{
    private int cacheDbId = 11;
    private string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? string.Empty;

    public CouponQuery(
        ICouponRepository couponRepository,
        IRedisService redisService)
    {
        Name = nameof(Coupon);
        Description = $"{nameof(Coupon)} description";

        Field<CouponType, Coupon>(name: "coupon")
            .Description("coupon type description")
            .Argument<IdGraphType>("id")
            .ResolveAsync(context =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for id"));
                    return null;
                }

                var loader = context.RequestServices.GetRequiredService<CouponBatchDataLoader>();
                return loader.LoadAsync(id);
            });

        Field<ListGraphType<CouponType>>(name: "allCoupons")
            .Description("Coupon type description")
            .ResolveAsync(async (context) =>
            {
                var cacheKey = CacheExtensions.GetCacheKey("allCoupons", className, null);
                return await redisService.GetAsync(cacheKey, cacheDbId,
                                                   new TimeSpan(2, 30, 0).Minutes,
                                                   async () =>
                                                   {
                                                       return await couponRepository.GetAllAsync();
                                                   });
            });

        Field<ListGraphType<CouponType>>(name: "couponsByFilter")
            .Description("Coupon type description")
            .ResolveAsync(async (context) =>
            {
                var rule = context.GetArgument<string>("string");
                if (string.IsNullOrWhiteSpace(rule))
                {
                    context.Errors.Add(new ExecutionError("Wrong value for id"));
                    return null;
                }

                var jsonDocument = JsonDocument.Parse(rule);
                var parser = new JsonExpressionParser();
                var expression = parser.ParseExpressionOf<Coupon>(jsonDocument);
                return await couponRepository.GetAllByFilterAsync(expression);
            });

        Field<RuleModelType>(name: "getRuleModel")
            .Description("Get rule model")
            .Resolve(context =>
            {
                var cacheKey = CacheExtensions.GetCacheKey("getRuleModel", className, null);
                var result = redisService.Get(cacheKey, cacheDbId,
                                          new TimeSpan(2, 30, 0).Minutes,
                                          () => RuleModelBuilder.GetModelRule(typeof(CampaignItem)));

                return result;
            });
    }
}
