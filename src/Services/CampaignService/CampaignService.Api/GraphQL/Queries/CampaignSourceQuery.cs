using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.Repository.Abstract;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Queries;

public class CampaignSourceQuery : ObjectGraphType<CampaignSource>
{
    public CampaignSourceQuery(
        ICampaignSourceRepository campaignSourceRepository)
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
            .ResolveAsync(async (context) => await campaignSourceRepository.GetAllAsync());

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

                var result = await campaignSourceRepository.GetAllByCampaignIdAsync(id);
                return result;
            });
    }
}
