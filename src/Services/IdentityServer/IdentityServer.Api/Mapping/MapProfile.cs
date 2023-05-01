using AutoMapper;
using IdentityServer.Api.Dtos.ApiResourceDtos;
using IdentityServer.Api.Dtos.ApiScopeDtos;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Dtos.ClientDtos;
using IdentityServer.Api.Dtos.IdentityResourceDtos;
using IdentityServer.Api.Dtos.UserDtos;
using IdentityServer.Api.Entities.Identity;

namespace IdentityServer.Api.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            #region Client
            CreateMap<IdentityServer4.Models.Client, IdentityServer4.EntityFramework.Entities.Client>()
                .ForMember(c => c.ClientSecrets, opt => opt.MapFrom(o => o.ClientSecrets)).ReverseMap();

            CreateMap<ClientAddDto, IdentityServer4.Models.Client>()
                .ForMember(c => c.ClientSecrets, opt => opt.MapFrom(o => o.Secrets)).ReverseMap();

            CreateMap<ClientAddDto, IdentityServer4.EntityFramework.Entities.Client>();

            CreateMap<ClientDto, IdentityServer4.Models.Client>()
                .ForMember(c => c.ClientSecrets, opt => opt.MapFrom(o => o.Secrets)).ReverseMap();

            CreateMap<IdentityServer4.EntityFramework.Entities.Client, ClientDto>()
                .ForMember(c => c.Secrets, opt => opt.MapFrom(o => o.ClientSecrets))
                .ForMember(c => c.AllowedGrantTypes, opt => opt.MapFrom(o => o.AllowedGrantTypes.Select(a => a.GrantType).ToList()))
                .ForMember(c => c.RedirectUris, opt => opt.MapFrom(o => o.RedirectUris.Select(r => r.RedirectUri).ToList()))
                .ForMember(c => c.PostLogoutRedirectUris, opt => opt.MapFrom(o => o.PostLogoutRedirectUris.Select(p => p.PostLogoutRedirectUri).ToList()))
                .ForMember(c => c.AllowedScopes, opt => opt.MapFrom(o => o.AllowedScopes.Select(a => a.Scope)));

            CreateMap<ClientDto, IdentityServer4.EntityFramework.Entities.Client>()
                .ForMember(c => c.ClientSecrets, opt => opt.MapFrom(o => o.Secrets));
            #endregion
            #region Secret
            CreateMap<IdentityServer4.Models.Secret, IdentityServer4.EntityFramework.Entities.Secret>()
                .ReverseMap();
            #endregion
            #region ApiScope
            CreateMap<ApiScopeAddDto, IdentityServer4.Models.ApiScope>()
                .ForMember(a => a.Properties, opt => opt.MapFrom(ap => ap.Properties.ToDictionary(t => t.Key, t => t.Value)))
                .ForMember(a => a.UserClaims, opt => opt.MapFrom(ap => ap.UserClaims));

            CreateMap<IdentityServer4.Models.ApiScope, ApiScopeDto>()
                .ForMember(a => a.Properties, opt => opt.MapFrom(ap => ap.Properties.Select(p => 
                new PropertyDto() { Key = p.Key, Value = p.Value })))
                .ForMember(a => a.UserClaims, opt => opt.MapFrom(ap => ap.UserClaims));

            CreateMap<IdentityServer4.EntityFramework.Entities.ApiScope, ApiScopeDto>()
                .ForMember(a => a.Properties, opt => opt.MapFrom(ap => ap.Properties.Select(b => 
                new PropertyDto() { Key = b.Key, Value = b.Value })))
                .ForMember(a => a.UserClaims, opt => opt.MapFrom(ap => ap.UserClaims.Select(b => b.Type)));
            #endregion
            #region ApiResource
            CreateMap<ApiResourceAddDto, IdentityServer4.Models.ApiResource>()
                .ForMember(a => a.Scopes, opt => opt.MapFrom(ar => ar.Scopes))
                .ForMember(a => a.Properties, opt => opt.MapFrom(ar => ar.Properties.ToDictionary(p => p.Key, p => p.Value)))
                .ForMember(a => a.UserClaims, opt => opt.MapFrom(ar => ar.UserClaims))
                .ForMember(a => a.AllowedAccessTokenSigningAlgorithms, opt => opt.MapFrom(ar => ar.AllowedAccessTokenSigningAlgorithms))
                .ForMember(a => a.ApiSecrets, opt => opt.MapFrom(ar => ar.Secrets));

            CreateMap<IdentityServer4.EntityFramework.Entities.ApiResource, ApiResourceDto>()
                .ForMember(a => a.Scopes, opt => opt.MapFrom(ar => ar.Scopes.Select(sc => sc.Scope)))
                .ForMember(a => a.Properties, opt => opt.MapFrom(ar => ar.Properties.Select(p => 
                new PropertyDto() { Key = p.Key, Value = p.Value } )))
                .ForMember(a => a.AllowedAccessTokenSigningAlgorithms, opt => opt.MapFrom(ar => ar.AllowedAccessTokenSigningAlgorithms))
                .ForMember(a => a.Secrets, opt => opt.MapFrom(ar => ar.Secrets))
                .ForMember(a => a.UserClaims, opt => opt.MapFrom(ar => ar.UserClaims.Select(u => u.Type)));

            CreateMap<IdentityServer4.Models.ApiResource, ApiResourceDto>()
                .ForMember(a => a.Scopes, opt => opt.MapFrom(ar => ar.Scopes))
                .ForMember(a => a.Properties, opt => opt.MapFrom(ar => ar.Properties.Select(p =>
                new PropertyDto() { Key = p.Key, Value = p.Value })))
                .ForMember(a => a.AllowedAccessTokenSigningAlgorithms, opt => opt.MapFrom(ar => ar.AllowedAccessTokenSigningAlgorithms))
                .ForMember(a => a.Secrets, opt => opt.MapFrom(ar => ar.ApiSecrets))
                .ForMember(a => a.UserClaims, opt => opt.MapFrom(ar => ar.UserClaims));
            #endregion
            #region IdentityResource
            CreateMap<IdentityResourceAddDto, IdentityServer4.Models.IdentityResource>()
                .ForMember(id => id.Properties, opt => opt.MapFrom(idr => idr.Properties.ToDictionary(p => p.Key, p => p.Value)))
                .ForMember(id => id.UserClaims, opt => opt.MapFrom(idr => idr.UserClaims));

            CreateMap<IdentityServer4.EntityFramework.Entities.IdentityResource, IdentityResourceDto>()
                .ForMember(id => id.Properties, opt => opt.MapFrom(idr => idr.Properties.Select(p => 
                new PropertyDto() { Key = p.Key, Value = p.Value })))
                .ForMember(id => id.UserClaims, opt => opt.MapFrom(idr => idr.UserClaims.Select(u => u.Type)));

            CreateMap<IdentityServer4.Models.IdentityResource, IdentityResourceDto>()
                .ForMember(id => id.Properties, opt => opt.MapFrom(idr => idr.Properties.Select(p =>
                new PropertyDto() { Key = p.Key, Value = p.Value })))
                .ForMember(id => id.UserClaims, opt => opt.MapFrom(idr => idr.UserClaims));
            #endregion
            #region User
            CreateMap<UserAddDto, User>();

            CreateMap<UserAddDto, UserDto>();

            CreateMap<User, UserDto>();

            CreateMap<UserUpdateDto, User>()
                .ForMember(s => s.Name, opt => opt.MapFrom(u => u.Name))
                .ForMember(s => s.Surname, opt => opt.MapFrom(u => u.Surname))
                .ForMember(s => s.UserName, opt => opt.MapFrom(u => u.UserName))
                .ForAllOtherMembers(opt => opt.Ignore());
            #endregion
        }
    }
}
