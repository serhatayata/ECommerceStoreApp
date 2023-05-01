using IdentityServer.Api.Dtos.Base.Abstract;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Base
{
    public interface IAddAsyncService<T, A> where A : class, IAddDto
                                            where T : class, IDto
    {
        /// <summary>
        /// Add <typeparamref name="A"/>
        /// </summary>
        /// <param name="model"><see cref="A"/></param>
        /// <returns><typeparamref name="T"/></returns>
        Task<DataResult<T>> AddAsync(A model);
    }
}
