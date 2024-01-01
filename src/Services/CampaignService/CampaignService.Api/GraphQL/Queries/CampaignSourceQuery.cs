using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Services.Cache.Abstract;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using System.Reflection;

namespace CampaignService.Api.GraphQL.Queries;

public class CampaignSourceQuery : ObjectGraphType<CampaignSource>
{
    private int cacheDbId = 11;
    private string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? string.Empty;

    public CampaignSourceQuery(
        ICampaignSourceRepository campaignSourceRepository,
        IRedisService redisService)
    {
        Name = nameof(CampaignSourceQuery);
        Description = $"{nameof(CampaignSourceQuery)} description";

        //Sample of batch data loader
        Field<CampaignSourceType, CampaignSource>(name: "campaignSource")
            .Description("campaign source type description")
            .Argument<IdGraphType>("id")
            .ResolveAsync(context =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for campaign id"));
                    return null;
                }

                var loader = context.RequestServices.GetRequiredService<CampaignSourceBatchDataLoader>();
                return loader.LoadAsync(id);
            });

        Field<ListGraphType<CampaignSourceType>>(name: "allCampaignSources")
        .Description("All campaign sources type description")
            .ResolveAsync(async (context) =>
            {
                var cacheKey = CacheExtensions.GetCacheKey("allCampaignSources", className);
                return await redisService.GetAsync(cacheKey, cacheDbId,
                                                   new TimeSpan(2, 30, 0).Minutes,
                                                   async () => await campaignSourceRepository.GetAllAsync());
            });

        Field<ListGraphType<CampaignSourceType>>(name: "allByCampaignId")
            .Description("Get all sources by campaign id")
            .Arguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "campaignId" })
            .ResolveAsync(async (context) =>
            {
                var id = context.GetArgument<int>("campaignId");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for campaign id"));
                    return null;
                }

                var cacheKey = CacheExtensions.GetCacheKey("allByCampaignId", className, id.ToString());
                return await redisService.GetAsync(cacheKey, cacheDbId,
                                                   new TimeSpan(2, 30, 0).Minutes,
                                                   async () => await campaignSourceRepository.GetAllByCampaignIdAsync(id));
            });
    }
}
