using IdentityServer.Api.Dtos.Base.Abstract;
using IdentityServer.Api.Models.IncludeOptions;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base
{
    public interface IGetAsyncService<T,R,Y> where T : class, IDto
                                             where R : class, IDto
                                             where Y : IBaseIncludeOptions
    {
        /// <summary>
        /// Get <typeparamref name="T"/> by <typeparamref name="R"/>
        /// </summary>
        /// <param name="id"><typeparamref name="R"/></param>
        /// <returns><see cref="DataResult{T}<"/></returns>
        Task<DataResult<T>> GetAsync(R model, Y options);
    }
}
