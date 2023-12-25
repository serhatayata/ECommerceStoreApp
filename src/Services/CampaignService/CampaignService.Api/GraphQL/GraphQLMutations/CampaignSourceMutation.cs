using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.GraphQLTypes.Inputs;
using CampaignService.Api.GraphQL.GraphQLTypes;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Repository.Concrete;
using GraphQL;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLMutations;

public class CampaignSourceMutation : ObjectGraphType
{
    public CampaignSourceMutation(
        ICampaignSourceRepository campaignSourceRepository)
    {
        Field<CampaignSourceType>("createCampaignSource")
            .Arguments(new QueryArgument<NonNullGraphType<CampaignSourceInputType>> { Name = "campaignSource" })
            .ResolveAsync(async (context) =>
            {
                var campaignSource = context.GetArgument<CampaignSource>("campaignSource");
                var result = await campaignSourceRepository.CreateAsync(campaignSource);
                return result;
            });

        Field<CampaignSourceType>("updateCampaignSource")
            .Arguments(
                new QueryArgument<NonNullGraphType<CampaignSourceInputType>> { Name = "campaignSource" })
            .ResolveAsync(async (context) =>
            {
                var campaignSource = context.GetArgument<CampaignSource>("campaignSource");
                if (campaignSource == null || campaignSource?.Id == default)
                {
                    context.Errors.Add(new ExecutionError("Couldn't find parameters for update"));
                    return null;
                }

                return await campaignSourceRepository.UpdateAsync(campaignSource);
            });

        Field<BooleanGraphType>("deleteCampaignSource")
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

                return await campaignSourceRepository.DeleteAsync(id);
            });
    }
}
