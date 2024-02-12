using CampaignService.Api.Entities;
using CampaignService.Api.Repository.Abstract;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Queries;

public class CampaignRuleQuery : ObjectGraphType<CampaignRule>
{
    public CampaignRuleQuery(
        ICampaignRuleRepository campaignRuleRepository)
    {
        
    }
}
