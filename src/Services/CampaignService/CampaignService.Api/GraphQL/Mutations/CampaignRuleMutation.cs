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

public class CampaignRuleMutation : ObjectGraphType<CampaignRule>
{
    public CampaignRuleMutation(
        ICampaignRuleRepository campaignRuleRepository,
        IMapper mapper)
    {
        Field<CampaignRuleType>("createCampaignRule")
            .Arguments(new QueryArgument<NonNullGraphType<CampaignRuleInputType>> { Name = "campaignRule" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<CampaignRule, CampaignRuleInput>("campaignRule");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                    .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var campaignRule = mapper.Map<CampaignRule>(validationResult.Model);
                var result = await campaignRuleRepository.CreateAsync(campaignRule);
                return result;
            });

        Field<CampaignType>("updateCampaignRule")
            .Arguments(
                new QueryArgument<NonNullGraphType<CampaignRuleInputType>> { Name = "campaignRule" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<CampaignRule, CampaignRuleInput>("campaignRule");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var campaignRule = mapper.Map<CampaignRule>(validationResult.Model);
                return await campaignRuleRepository.UpdateAsync(campaignRule);
            });

        Field<BooleanGraphType>("deleteCampaignRule")
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

                return await campaignRuleRepository.DeleteAsync(id);
            });
    }
}
