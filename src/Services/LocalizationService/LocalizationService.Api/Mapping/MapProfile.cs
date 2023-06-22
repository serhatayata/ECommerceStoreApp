using AutoMapper;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.LanguageModels;
using LocalizationService.Api.Models.MemberModels;
using LocalizationService.Api.Models.ResourceModels;

namespace LocalizationService.Api.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            #region Language
            CreateMap<Language, LanguageModel>().ReverseMap();
            CreateMap<Language, LanguageAddModel>().ReverseMap();
            CreateMap<Language, LanguageUpdateModel>().ReverseMap();
            #endregion
            #region Member
            CreateMap<Member, MemberModel>().ReverseMap();
            CreateMap<Member, MemberAddModel>().ReverseMap();
            CreateMap<Member, MemberUpdateModel>().ReverseMap();
            #endregion
            #region Resource
            CreateMap<Resource, ResourceModel>().ReverseMap();
            CreateMap<Resource, ResourceAddModel>().ReverseMap();
            CreateMap<Resource, ResourceUpdateModel>().ReverseMap();
            CreateMap<Resource, ResourceCacheModel>().ReverseMap();
            #endregion
        }
    }
}
