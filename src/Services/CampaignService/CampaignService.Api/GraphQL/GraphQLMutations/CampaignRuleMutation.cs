using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.GraphQLTypes.Inputs;
using CampaignService.Api.GraphQL.GraphQLTypes;
using CampaignService.Api.Repository.Concrete;
using GraphQL;
using GraphQL.Types;
using CampaignService.Api.Repository.Abstract;

namespace CampaignService.Api.GraphQL.GraphQLMutations;

public class CampaignRuleMutation : ObjectGraphType<CampaignRule>
{
    public CampaignRuleMutation(
        ICampaignRuleRepository campaignRuleRepository)
    {
        Field<CampaignRuleType>("createCampaignRule")
            .Arguments(new QueryArgument<NonNullGraphType<CampaignRuleInputType>> { Name = "campaignRule" })
        .ResolveAsync(async (context) =>
            {
                var campaignRule = context.GetArgument<CampaignRule>("campaignRule");
                var result = await campaignRuleRepository.CreateAsync(campaignRule);
                return result;
            });

        Field<CampaignRuleType>("updateCampaignRule")
            .Arguments(
                new QueryArgument<NonNullGraphType<CampaignRuleInputType>> { Name = "campaignRule" })
            .ResolveAsync(async (context) =>
            {
                var campaignRule = context.GetArgument<CampaignRule>("campaignRule");
                if (campaignRule == null || campaignRule?.Id == default)
                {
                    context.Errors.Add(new ExecutionError("Couldn't find parameters for update"));
                    return null;
                }

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
