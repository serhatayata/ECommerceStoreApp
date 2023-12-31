using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Repository.Abstract;
using GraphQL;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Mutations;

public class CampaignItemMutation : ObjectGraphType<CampaignItem>
{
    public CampaignItemMutation(
        ICampaignItemRepository campaignItemRepository)
    {
        Field<CampaignItemType>("createCampaignItem")
            .Arguments(new QueryArgument<NonNullGraphType<CampaignItemInputType>> { Name = "campaignItem" })
            .ResolveAsync(async (context) =>
            {
                var campaignItem = context.GetArgument<CampaignItem>("campaignItem");
                var result = await campaignItemRepository.CreateAsync(campaignItem);
                return result;
            });

        Field<CampaignItemType>("updateCampaignItem")
            .Arguments(
                new QueryArgument<NonNullGraphType<CampaignItemInputType>> { Name = "campaignItem" })
            .ResolveAsync(async (context) =>
            {
                var campaignItem = context.GetArgument<CampaignItem>("campaignItem");
                if (campaignItem == null || campaignItem?.Id == default)
                {
                    context.Errors.Add(new ExecutionError("Couldn't find parameters for update"));
                    return null;
                }

                return await campaignItemRepository.UpdateAsync(campaignItem);
            });

        Field<BooleanGraphType>("deleteCampaignItem")
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

                return await campaignItemRepository.DeleteAsync(id);
            });
    }
}