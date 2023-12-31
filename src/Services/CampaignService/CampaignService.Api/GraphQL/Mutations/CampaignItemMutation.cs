using AutoMapper;
using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.GraphQL.Types.InputTypes;
using CampaignService.Api.Repository.Abstract;
using GraphQL;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Mutations;

public class CampaignItemMutation : ObjectGraphType<CampaignItem>
{
    public CampaignItemMutation(
        ICampaignItemRepository campaignItemRepository,
        IMapper mapper)
    {
        Field<CampaignItemType>("createCampaignItem")
            .Arguments(new QueryArgument<NonNullGraphType<CampaignItemInputType>> { Name = "campaignItem" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<CampaignItem, CampaignItemInput>("campaignItem");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var campaignItem = mapper.Map<CampaignItem>(validationResult.Model);
                var result = await campaignItemRepository.CreateAsync(campaignItem);
                return result;
            });

        Field<CampaignItemType>("updateCampaignItem")
            .Arguments(
                new QueryArgument<NonNullGraphType<CampaignItemInputType>> { Name = "campaignItem" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<CampaignItem, CampaignItemInput>("campaignItem");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var campaignItem = mapper.Map<CampaignItem>(validationResult.Model);
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