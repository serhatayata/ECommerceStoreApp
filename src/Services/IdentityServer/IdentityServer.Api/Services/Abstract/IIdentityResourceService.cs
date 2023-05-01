using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IdentityResourceModels;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IIdentityResourceService : IBaseService<IdentityResourceModel, StringModel, IdentityResourceIncludeOptions>,
                                                IAddService<IdentityResourceModel, IdentityResourceAddModel>,
                                                IDeleteService<StringModel>
    {
    }
}
