namespace LocalizationService.Api.Services.Base
{
    public interface IGetAllAsyncService<T, Y> where T : class, IModel
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
