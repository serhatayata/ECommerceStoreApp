using IdentityServer.Api.Dtos;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base
{
    public interface IBaseService<T,R> where T : class
    {
        /// <summary>
        /// Add <typeparamref name="T"/>
        /// </summary>
        /// <param name="model"><see cref="T"/></param>
        /// <returns><typeparamref name="T"/></returns>
        Task<DataResult<T>> AddAsync(T model);
        /// <summary>
        /// Update <typeparamref name="T"/>
        /// </summary>
        /// <param name="model">Model <typeparamref name="T"/></param>
        /// <returns><typeparamref name="T"/></returns>
        DataResult<T> Update(T model);
        /// <summary>
        /// Delete <typeparamref name="T"/>
        /// </summary>
        /// <param name="id"><typeparamref name="R"/></param>
        /// <returns><see cref="Result"/></returns>
        Result DeleteAsync(R id);
        /// <summary>
        /// Get <typeparamref name="T"/> by <typeparamref name="R"/>
        /// </summary>
        /// <param name="id"><typeparamref name="R"/></param>
        /// <returns><see cref="DataResult{T}<"/></returns>
        Task<DataResult<T>> GetAsync(R id);
        /// <summary>
        /// Get list of <typeparamref name="T"/>
        /// </summary>
        /// <param name="id"><typeparamref name="R"/></param>
        /// <returns><see cref="List{DataResult{T}}"/></returns>
        Task<DataResult<List<T>>> GetAllAsync();
    }
}
