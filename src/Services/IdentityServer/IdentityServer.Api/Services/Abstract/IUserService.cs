using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.User;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Base;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IUserService : IGetAllAsyncService<UserModel, UserIncludeOptions>,
                                    IGetAsyncService<UserModel, StringModel, UserIncludeOptions>,
                                    IAddAsyncService<UserModel, UserAddModel>,
                                    IUpdateAsyncService<UserModel, UserUpdateModel>,
                                    IDeleteAsyncService<StringModel>
    {

    }
}
