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

public class CampaignMutation : ObjectGraphType<Campaign>
{
    public CampaignMutation(
        ICampaignRepository campaignRepository,
        IMapper mapper)
    {
        Field<CampaignType>("createCampaign")
            .Arguments(new QueryArgument<NonNullGraphType<CampaignInputType>> { Name = "campaign" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<Campaign, CampaignInput>("campaign");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var campaign = mapper.Map<Campaign>(validationResult.Model);
                var result = await campaignRepository.CreateAsync(campaign);
                return result;
            });

        Field<CampaignType>("updateCampaign")
            .Arguments(
                new QueryArgument<NonNullGraphType<CampaignInputType>> { Name = "campaign" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<Campaign, CampaignInput>("campaign");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var campaign = mapper.Map<Campaign>(validationResult.Model);
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
