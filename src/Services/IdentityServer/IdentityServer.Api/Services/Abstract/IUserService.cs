using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Dtos.UserDtos;
using IdentityServer.Api.Models.IncludeOptions.User;
using IdentityServer.Api.Services.Base;

namespace IdentityServer.Api.Services.Abstract
{
    public interface IUserService : IGetAllAsyncService<UserDto, UserIncludeOptions>,
                                    IGetAsyncService<UserDto, StringDto, UserIncludeOptions>,
                                    IAddAsyncService<UserDto, UserAddDto>,
                                    IUpdateAsyncService<UserDto, UserUpdateDto>,
                                    IDeleteAsyncService<StringDto>
    {

    }
}
