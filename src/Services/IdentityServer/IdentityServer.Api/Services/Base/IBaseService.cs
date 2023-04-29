using IdentityServer.Api.Dtos.Base.Abstract;
using IdentityServer.Api.Models.IncludeOptions;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base
{
    public interface IBaseService<T,A,R,Y> where T : class, IDto
                                           where A : class, IAddDto
                                           where Y : IBaseIncludeOptions
    {
        /// <summary>
        /// Add <typeparamref name="T"/>
        /// </summary>
        /// <param name="model"><see cref="T"/></param>
        /// <param name="model"><see cref="A"/></param>
        /// <returns><typeparamref name="T"/></returns>
        DataResult<T> Add(A model);
        /// <summary>
        /// Delete <typeparamref name="T"/>
        /// </summary>
        /// <param name="id"><typeparamref name="R"/></param>
        /// <returns><see cref="Result"/></returns>
        Result Delete(R model);
        /// <summary>
        /// Get <typeparamref name="T"/> by <typeparamref name="R"/>
        /// </summary>
        /// <param name="id"><typeparamref name="R"/></param>
        /// <returns><see cref="DataResult{T}<"/></returns>
        DataResult<T> Get(R id, Y options);
        /// <summary>
        /// Get list of <typeparamref name="T"/>
        /// </summary>
        /// <param name="id"><typeparamref name="R"/></param>
        /// <returns><see cref="List{DataResult{T}}"/></returns>
        DataResult<List<T>> GetAll(Y options);
    }
}
