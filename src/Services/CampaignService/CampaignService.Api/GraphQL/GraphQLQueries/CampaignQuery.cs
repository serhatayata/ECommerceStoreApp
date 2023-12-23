using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.GraphQLTypes;
using CampaignService.Api.Repository.Abstract;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLQueries;

public class CampaignQuery : ObjectGraphType<Campaign>
{
    public CampaignQuery(ICampaignRepository campaignRepository)
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
            .ResolveAsync(async (context) => await campaignRepository.GetAllAsync());


    }
}
