using IdentityServer.Api.Models.ApiResourceModels;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IApiResourceService : IBaseService<ApiResourceModel, StringModel, ApiResourceIncludeOptions>,
                                           IAddService<ApiResourceModel, ApiResourceAddModel>,
                                           IDeleteService<StringModel>
    {
    }
}
