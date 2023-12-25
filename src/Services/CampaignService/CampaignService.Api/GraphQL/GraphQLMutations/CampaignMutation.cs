using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.GraphQLTypes;
using CampaignService.Api.GraphQL.GraphQLTypes.Inputs;
using CampaignService.Api.Repository.Abstract;
using GraphQL;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLMutations;

public class CampaignMutation : ObjectGraphType
{
    public CampaignMutation(
        ICampaignRepository campaignRepository)
    {
        Field<CampaignType>("createCampaign")
            .Arguments(new QueryArgument<NonNullGraphType<CampaignInputType>> { Name = "campaign" })
            .ResolveAsync(async (context) =>
            {
                var campaign = context.GetArgument<Campaign>("campaign");
                var result = await campaignRepository.CreateAsync(campaign);
                return result;
            });

        Field<CampaignType>("updateCampaign")
            .Arguments(
                new QueryArgument<NonNullGraphType<CampaignInputType>> { Name = "campaign" })
            .ResolveAsync(async (context) =>
            {
                var campaign = context.GetArgument<Campaign>("campaign");
                if (campaign == null || campaign?.Id == default)
                {
                    context.Errors.Add(new ExecutionError("Couldn't find parameters for update"));
                    return null;
                }

                return await campaignRepository.UpdateAsync(campaign);
            });

        Field<BooleanGraphType>("deleteCampaign")
            .Arguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" })
            .ResolveAsync(async (context) =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Couldn't find parameters for delete"));
                    return false;
                }

                return await campaignRepository.DeleteAsync(id);
            });
    }
}
