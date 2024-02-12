using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.Models.CampaignRule;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Services.Cache.Abstract;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using System.Reflection;

namespace CampaignService.Api.GraphQL.Queries;

public class CampaignRuleQuery : ObjectGraphType<CampaignRule>
{
    private int cacheDbId = 11;
    private string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? string.Empty;

    public CampaignRuleQuery(
        ICampaignRuleRepository campaignRuleRepository,
        IRedisService redisService)
    {
        Field<CampaignRuleType, CampaignRule>(name: "campaignRule")
            .Description("campaign rule description")
            .Argument<IdGraphType>("id")
            .ResolveAsync(context =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for id"));
                    return null;
                }

                var loader = context.RequestServices.GetRequiredService<CampaignRuleBatchDataLoader>();
                return loader.LoadAsync(id);
            });

        Field<ListGraphType<CampaignRuleType>>(name: "allCampaignRules")
            .Description("Campaign rule description")
            .ResolveAsync(async (context) =>
            {
                var cacheKey = CacheExtensions.GetCacheKey("allCampaignRules", className, null);
                return await redisService.GetAsync(cacheKey, cacheDbId,
                                                   new TimeSpan(2, 30, 0).Minutes,
                                                   async () =>
                                                   {
                                                       return await campaignRuleRepository.GetAllAsync();
                                                   });
            });

        Field<ListGraphType<CampaignRuleType>>(name: "campaignRulesByFilter")
            .Description("Campaign rule by filter description")
            .ResolveAsync(async (context) =>
            {
                var filter = context.GetArgument<CampaignRuleGetByFilterModel>("filter");
                if (filter == null)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for id"));
                    return null;
                }

                return await campaignRuleRepository.GetAllByFilterAsync(filter);
            });
    }
}
