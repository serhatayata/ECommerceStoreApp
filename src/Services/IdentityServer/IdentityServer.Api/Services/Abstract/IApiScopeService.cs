using IdentityServer.Api.Dtos;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IApiScopeService : IBaseService<ApiScopeDto, int, ApiScopeIncludeOptions>
    {

    }
}
