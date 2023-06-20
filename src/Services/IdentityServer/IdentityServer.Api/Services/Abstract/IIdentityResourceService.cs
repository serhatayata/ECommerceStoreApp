using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IdentityResourceModels;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base.Abstract;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IIdentityResourceService : IBaseService<IdentityResourceModel, StringModel, IdentityResourceIncludeOptions>,
                                                IAddService<IdentityResourceModel, IdentityResourceAddModel>,
                                                IDeleteService<StringModel>
    {
        DataResult<List<IdentityResourceModel>> Get(List<string> scopeNames, IdentityResourceIncludeOptions options);
    }
}
