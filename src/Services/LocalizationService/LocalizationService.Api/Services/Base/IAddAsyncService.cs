using LocalizationService.Api.Models.Base.Abstract;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.Base
{
    public interface IAddAsyncService<T, A> where A : class, IAddModel
                                            where T : class, IModel
    {
        /// <summary>
        /// Add <typeparamref name="A"/>
        /// </summary>
        /// <param name="model"><see cref="A"/></param>
        /// <returns><typeparamref name="T"/></returns>
        Task<DataResult<T>> AddAsync(A model);
    }
}
