using AutoMapper;
using IdentityServer.Api.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace IdentityServer.Api.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<ClientDto, IdentityServer4.Models.Client>().ReverseMap();
            CreateMap<ClientDto, IdentityServer4.EntityFramework.Entities.Client>().ReverseMap();

            CreateMap<ApiResourceDto, IdentityServer4.Models.ApiResource>().ReverseMap();
            CreateMap<ApiResourceDto, IdentityServer4.EntityFramework.Entities.ApiResource>().ReverseMap();

            CreateMap<ApiScopeDto, IdentityServer4.Models.ApiScope>().ReverseMap();
            CreateMap<ApiScopeDto, IdentityServer4.EntityFramework.Entities.ApiScope>().ReverseMap();
        }
    }
}
