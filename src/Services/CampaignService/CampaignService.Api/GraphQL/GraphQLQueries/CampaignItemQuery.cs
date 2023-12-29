using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.DataLoaders.BatchDataLoaders;
using CampaignService.Api.GraphQL.GraphQLTypes;
using CampaignService.Api.Models.Rules;
using CampaignService.Api.Repository.Abstract;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.GraphQLQueries;

public class CampaignItemQuery : ObjectGraphType<CampaignItem>
{
    public CampaignItemQuery(
        ICampaignItemRepository campaignItemRepository)
    {
        Name = nameof(CampaignItemQuery);
        Description = $"{nameof(CampaignItemQuery)} description";

        //Sample of batch data loader
        Field<CampaignItemType, CampaignItem>(name: "campaignItem")
            .Description("campaign item type description")
            .Argument<IdGraphType>("id")
            .ResolveAsync(context =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for campaign id"));
                    return null;
                }

                var loader = context.RequestServices.GetRequiredService<CampaignItemBatchDataLoader>();
                return loader.LoadAsync(id);
            });

        Field<ListGraphType<CampaignItemType>>(name: "allCampaignItems")
            .Description("All campaign items type description")
            .ResolveAsync(async (context) => await campaignItemRepository.GetAllAsync());

        Field<ListGraphType<CampaignItemType>>(name: "allByCampaignId")
            .Description("Get all items by campaign id")
            .Arguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "campaignId" })
            .ResolveAsync(async (context) =>
            {
                var id = context.GetArgument<int>("campaignId");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Wrong value for campaign id"));
                    return null;
                }

                var result = await campaignItemRepository.GetAllByCampaignIdAsync(id);
                return result;
            });

        Field<ListGraphType<CampaignItemType>>(name: "allByRule")
            .Description("Get all items by rule")
            .Arguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "rule" })
            .ResolveAsync(async (context) =>
            {
                var rule = context.GetArgument<string>("rule");
                if (string.IsNullOrWhiteSpace(rule))
                {
                    context.Errors.Add(new ExecutionError("Wrong value for rule"));
                    return null;
                }

                var result = await campaignItemRepository.GetAllByRulesync(rule);
                return result;
            });

        Field<RuleModel>(name: "getRuleModel")
            .Description("Get rule model")
            .ResolveAsync(async (context) =>
            {
                var rule = context.GetArgument<string>("rule");
                if (string.IsNullOrWhiteSpace(rule))
                {
                    context.Errors.Add(new ExecutionError("Wrong value for rule"));
                    return null;
                }

                var result = await campaignItemRepository.GetAllByRulesync(rule);
                return result;
            });
    }
}
