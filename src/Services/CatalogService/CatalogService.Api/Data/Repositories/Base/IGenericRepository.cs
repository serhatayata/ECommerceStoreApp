using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Base
{
    public interface IGenericRepository<T,R> where T : class
    {
        Task<DataResult<T>> GetAsync(R model);
        Task<DataResult<T>> GetAsync(System.Linq.Expressions.Expression<Func<T,bool>> predicate);
        Task<DataResult<IReadOnlyList<T>>> GetAllAsync();
        Task<DataResult<IReadOnlyList<T>>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        Task<Result> AddAsync(T entity);
        Task<Result> UpdateAsync(T entity);
        Task<Result> DeleteAsync(R model);
    }
}
