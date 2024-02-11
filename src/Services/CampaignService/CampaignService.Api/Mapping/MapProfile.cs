using AutoMapper;
using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.Types.Inputs;

namespace CampaignService.Api.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Campaign, CampaignInput>().ReverseMap();
        CreateMap<CampaignItem, CampaignItemInput>().ReverseMap();
        CreateMap<CampaignSource, CampaignSourceInput>().ReverseMap();
        CreateMap<CampaignRule, CampaignRuleInput>().ReverseMap();
    }
}
