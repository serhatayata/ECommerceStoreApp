using IdentityServer.Api.Models.ApiResourceModels;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IApiResourceService : IBaseService<ApiResourceModel, StringModel, ApiResourceIncludeOptions>,
                                           IAddService<ApiResourceModel, ApiResourceAddModel>,
                                           IDeleteService<StringModel>
    {
        DataResult<List<ApiResourceModel>> Get(List<string> apiResources, Models.IncludeOptions.Account.ApiResourceIncludeOptions options);

        DataResult<List<ApiResourceModel>> GetByApiScopeNames(List<string> apiScopeNames, Models.IncludeOptions.Account.ApiResourceIncludeOptions options);
    }
}
