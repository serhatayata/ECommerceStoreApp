﻿using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Repository.Concrete;
using CampaignService.Api.Services.Cache.Abstract;
using CampaignService.Api.Utilities;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using System.Reflection;

namespace CampaignService.Api.GraphQL.Queries;

public class CampaignItemQuery : ObjectGraphType<CampaignItem>
{
    private int cacheDbId = 11;
    private string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? string.Empty;

    public CampaignItemQuery(
        ICampaignItemRepository campaignItemRepository,
        IRedisService redisService)
    {
        Name = nameof(CampaignItemQuery);
        Description = $"{nameof(CampaignItemQuery)} description";

        //Sample of batch data loader
        Field<CampaignItemType, CampaignItem>(name: "campaignItem")
            .Description("campaign item type description")
            .Argument<IdGraphType>("id")
            .ResolveAsync(context =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for campaign id"));
                    return null;
                }

                var loader = context.RequestServices.GetRequiredService<CampaignItemBatchDataLoader>();
                return loader.LoadAsync(id);
            });

        Field<ListGraphType<CampaignItemType>>(name: "allCampaignItems")
            .Description("All campaign items type description")
            .ResolveAsync(async (context) =>
            {
                var cacheKey = CacheExtensions.GetCacheKey("allCampaignItems", className, null);
                return await redisService.GetAsync(cacheKey, cacheDbId, 
                                                   new TimeSpan(2, 30, 0).Minutes,
                                                   async () =>
                                                   {
                                                       return await campaignItemRepository.GetAllAsync();
                                                   });
                
            });

        Field<ListGraphType<CampaignItemType>>(name: "allByCampaignId")
            .Description("Get all items by campaign id")
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
                                                   async () => await campaignItemRepository.GetAllByCampaignIdAsync(id));
            });

        Field<ListGraphType<CampaignItemType>>(name: "allByRule")
            .Description("Get all items by rule")
            .Arguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "rule" })
            .ResolveAsync(async (context) =>
            {
                var rule = context.GetArgument<string>("rule");
                if (string.IsNullOrWhiteSpace(rule))
                {
                    context.Errors.Add(new ExecutionError("Wrong value for rule"));
                    return null;
                }

                var result = await campaignItemRepository.GetAllByRulesync(rule);
                return result;
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
