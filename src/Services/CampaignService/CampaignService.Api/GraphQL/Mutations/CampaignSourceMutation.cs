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

public class CampaignSourceMutation : ObjectGraphType<CampaignSource>
{
    public CampaignSourceMutation(
        ICampaignSourceRepository campaignSourceRepository,
        IMapper mapper)
    {
        Field<CampaignSourceType>("createCampaignSource")
            .Arguments(new QueryArgument<NonNullGraphType<CampaignSourceInputType>> { Name = "campaignSource" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<CampaignSource, CampaignSourceInput>("campaignSource");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var campaignSource = mapper.Map<CampaignSource>(validationResult.Model);
                var result = await campaignSourceRepository.CreateAsync(campaignSource);
                return result;
            });

        Field<CampaignSourceType>("updateCampaignSource")
            .Arguments(
                new QueryArgument<NonNullGraphType<CampaignSourceInputType>> { Name = "campaignSource" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<CampaignSource, CampaignSourceInput>("campaignSource");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var campaignSource = mapper.Map<CampaignSource>(validationResult.Model);
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
