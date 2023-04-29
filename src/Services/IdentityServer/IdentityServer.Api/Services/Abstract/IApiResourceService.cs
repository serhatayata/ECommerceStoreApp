using IdentityServer.Api.Dtos.ApiResourceDtos;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IApiResourceService : IBaseService<ApiResourceDto, ApiResourceAddDto, StringDto, ApiResourceIncludeOptions>
    {
    }
}
