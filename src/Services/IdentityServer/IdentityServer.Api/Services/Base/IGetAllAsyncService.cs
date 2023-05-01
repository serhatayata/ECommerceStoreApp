using IdentityServer.Api.Dtos.Base.Abstract;
using IdentityServer.Api.Models.IncludeOptions;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base
{
    public interface IGetAllAsyncService<T, Y> where T : class, IDto
                                               where Y : IBaseIncludeOptions
    {
        /// <summary>
        /// Get all <typeparamref name="T"/>
        /// </summary>
        /// <param name="id"><typeparamref name="R"/></param>
        /// <returns>List of <see cref="DataResult{List{T}}"/></returns>
        Task<DataResult<List<T>>> GetAllAsync(Y options);
    }
}
