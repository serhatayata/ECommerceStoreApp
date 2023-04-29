using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Dtos.IdentityResourceDtos;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IIdentityResourceService : IBaseService<IdentityResourceDto, IdentityResourceAddDto, StringDto, IdentityResourceIncludeOptions>
    {
    }
}
