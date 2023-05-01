using IdentityServer.Api.Dtos.Base.Abstract;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base
{
    public interface IUpdateService<T,U> where T : class, IDto
                                         where U : class, IUpdateDto
    {
        /// <summary>
        /// Update <typeparamref name="U"/>
        /// </summary>
        /// <param name="model"><see cref="T"/></param>
        /// <param name="model"><see cref="U"/></param>
        /// <returns><typeparamref name="T"/></returns>
        DataResult<T> Update(U model);
    }
}
