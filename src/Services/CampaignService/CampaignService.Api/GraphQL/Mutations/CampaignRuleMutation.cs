using AutoMapper;
using CampaignService.Api.Entities;
using CampaignService.Api.Repository.Abstract;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Mutations;

public class CampaignRuleMutation : ObjectGraphType<CampaignRule>
{
    public CampaignRuleMutation(
        ICampaignRuleRepository campaignRuleRepository,
        IMapper mapper)
    {
        
    }
}
