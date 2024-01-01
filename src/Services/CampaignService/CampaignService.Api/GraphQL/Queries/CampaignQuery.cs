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

public class CampaignQuery : ObjectGraphType<Campaign>
{
    private int cacheDbId = 11;
    private string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? string.Empty;

    public CampaignQuery(
        ICampaignRepository campaignRepository,
        IRedisService redisService)
    {
        Name = nameof(CampaignQuery);
        Description = $"{nameof(CampaignQuery)} description";

        //Sample of batch data loader
        Field<CampaignType, Campaign>(name: "campaign")
            .Description("campaign type description")
            .Argument<IdGraphType>("id")
            .ResolveAsync(context =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for id"));
                    return null;
                }

                var loader = context.RequestServices.GetRequiredService<CampaignBatchDataLoader>();
                return loader.LoadAsync(id);
            });

        Field<ListGraphType<CampaignType>>(name: "allCampaigns")
            .Description("Campaign type description")
            .ResolveAsync(async (context) =>
            {
                var cacheKey = CacheExtensions.GetCacheKey("allCampaigns", className, null);
                return await redisService.GetAsync(cacheKey, cacheDbId,
                                                   new TimeSpan(2, 30, 0).Minutes,
                                                   async () =>
                                                   {
                                                       return await campaignRepository.GetAllAsync();
                                                   });
            });
    }
}
