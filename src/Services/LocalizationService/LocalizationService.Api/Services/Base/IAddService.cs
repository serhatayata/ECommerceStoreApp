using LocalizationService.Api.Models.Base.Abstract;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.Base
{
    public interface IAddService<T,A> where A : class, IAddModel
                                        where T : class, IModel
    {
        /// <summary>
        /// Add <typeparamref name="A"/>
        /// </summary>
        /// <param name="model"><see cref="A"/></param>
        /// <returns><typeparamref name="T"/></returns>
        DataResult<T> Add(A model);
    }
}
