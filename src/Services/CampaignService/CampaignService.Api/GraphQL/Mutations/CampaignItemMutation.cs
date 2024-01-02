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
        ICampaignRepository campaignRepository,
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

                    return null;
                }

                var model = validationResult.Model;

                var campaign = await campaignRepository.GetAsync(id: validationResult.Model.CampaignId);
                if (campaign == null)
                {
                    context.Errors.Add(new ExecutionError("Campaign not exists"));
                    return null;
                }
                else if (campaign.ExpirationDate > DateTime.Now)
                {
                    context.Errors.Add(new ExecutionError("Campaign expiration date ended"));
                    return null;
                }
                else if (campaign.MaxUsage <= campaign.UsageCount)
                {
                    context.Errors.Add(new ExecutionError("Campaign usage limit error"));
                    return null;
                }

                var existingCampaignItem = campaignItemRepository.GetAsync(s => s.CampaignId == campaign.Id &&
                                                                                s.UserId == model.UserId);

                if (existingCampaignItem != null)
                {
                    context.Errors.Add(new ExecutionError("This campaign is already declared to this user"));
                    return null;
                }

                var campaignItem = mapper.Map<CampaignItem>(model);
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

                var campaign = await campaignRepository.GetAsync(id);
                if (campaign == null)
                {
                    context.Errors.Add(new ExecutionError("Campaign not found"));
                    return false;
                }

                campaign.UsageCount -= 1; 
                await campaignRepository.UpdateAsync(campaign);
                return await campaignItemRepository.DeleteAsync(id);
            });
    }
}