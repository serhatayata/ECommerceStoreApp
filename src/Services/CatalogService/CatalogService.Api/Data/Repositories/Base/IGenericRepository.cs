using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Base
{
    public interface IGenericRepository<T,R> where T : class
    {
        Task<DataResult<T>> GetAsync(R model);
        Task<DataResult<IReadOnlyList<T>>> GetAllAsync();
        Task<Result> AddAsync(T entity);
        Task<Result> UpdateAsync(T entity);
        Task<Result> DeleteAsync(R model);
    }
}
