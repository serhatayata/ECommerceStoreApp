using IdentityServer.Api.Dtos.ApiScopeDtos;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IApiScopeService : IBaseService<ApiScopeDto, StringDto, ApiScopeIncludeOptions>,
                                        IAddService<ApiScopeDto, ApiScopeAddDto>,
                                        IDeleteService<StringDto>
    {

    }
}
