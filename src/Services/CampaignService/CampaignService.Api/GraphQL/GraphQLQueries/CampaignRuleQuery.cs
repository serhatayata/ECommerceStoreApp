using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.GraphQLTypes;
using CampaignService.Api.Repository.Abstract;
using CampaignService.Api.Repository.Concrete;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLQueries;

public class CampaignRuleQuery : ObjectGraphType<CampaignRule>
{
    public CampaignRuleQuery(
        ICampaignRuleRepository campaignRuleRepository)
    {
        Name = nameof(CampaignRuleQuery);
        Description = $"{nameof(CampaignRuleQuery)} description";

        Field<CampaignRuleType, CampaignRule>(name: "campaignRule")
            .Description("campaign rule type description")
            .Argument<IdGraphType>("id")
            .ResolveAsync(context =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for campaign id"));
                    return null;
                }

                var loader = context.RequestServices.GetRequiredService<CampaignRuleBatchDataLoader>();
                return loader.LoadAsync(id);
            });

        Field<ListGraphType<CampaignRuleType>>(name: "allCampaignRules")
        .Description("All campaign rules type description")
            .ResolveAsync(async (context) => await campaignRuleRepository.GetAllAsync());
    }
}
