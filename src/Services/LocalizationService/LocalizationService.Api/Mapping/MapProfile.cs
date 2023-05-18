using AutoMapper;

namespace LocalizationService.Api.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            #region Client
            //CreateMap<IdentityServer4.Models.Client, IdentityServer4.EntityFramework.Entities.Client>()
            //    .ForMember(c => c.ClientSecrets, opt => opt.MapFrom(o => o.ClientSecrets)).ReverseMap();

            //CreateMap<ClientAddModel, IdentityServer4.Models.Client>()
            //    .ForMember(c => c.ClientSecrets, opt => opt.MapFrom(o => o.Secrets)).ReverseMap();

            //CreateMap<ClientAddModel, IdentityServer4.EntityFramework.Entities.Client>();

            //CreateMap<ClientModel, IdentityServer4.Models.Client>()
            //    .ForMember(c => c.ClientSecrets, opt => opt.MapFrom(o => o.Secrets)).ReverseMap();

            //CreateMap<IdentityServer4.EntityFramework.Entities.Client, ClientModel>()
            //    .ForMember(c => c.Secrets, opt => opt.MapFrom(o => o.ClientSecrets))
            //    .ForMember(c => c.AllowedGrantTypes, opt => opt.MapFrom(o => o.AllowedGrantTypes.Select(a => a.GrantType).ToList()))
            //    .ForMember(c => c.RedirectUris, opt => opt.MapFrom(o => o.RedirectUris.Select(r => r.RedirectUri).ToList()))
            //    .ForMember(c => c.PostLogoutRedirectUris, opt => opt.MapFrom(o => o.PostLogoutRedirectUris.Select(p => p.PostLogoutRedirectUri).ToList()))
            //    .ForMember(c => c.AllowedScopes, opt => opt.MapFrom(o => o.AllowedScopes.Select(a => a.Scope)));

            //CreateMap<ClientModel, IdentityServer4.EntityFramework.Entities.Client>()
            //    .ForMember(c => c.ClientSecrets, opt => opt.MapFrom(o => o.Secrets));
            #endregion
        }
    }
}
