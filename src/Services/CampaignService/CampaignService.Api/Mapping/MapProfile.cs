using AutoMapper;
using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Models.Coupon;

namespace CampaignService.Api.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Campaign, CampaignInput>().ReverseMap();
        CreateMap<CampaignItem, CampaignItemInput>().ReverseMap();
        CreateMap<CampaignSource, CampaignSourceInput>().ReverseMap();
        CreateMap<CampaignRule, CampaignRuleInput>().ReverseMap();
        CreateMap<Coupon, CouponInput>().ReverseMap();
        CreateMap<CouponItem, CouponItemInput>().ReverseMap();
        CreateMap<CouponUsage, Coupon>().ReverseMap();
    }
}
