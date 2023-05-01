using IdentityServer.Api.Models.Base.Abstract;
using IdentityServer.Api.Models.IncludeOptions;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base
{
    public interface IGetAsyncService<T,R,Y> where T : class, IModel
                                             where R : class, IModel
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
