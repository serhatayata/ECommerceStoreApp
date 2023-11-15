using OrderService.Api.Utilities.Results;

namespace OrderService.Api.Repositories.Base;

public interface IGenericRepository<T, R> where T : class
{
    Task<DataResult<T>> GetAsync(R model);
    Task<DataResult<IReadOnlyList<T>>> GetAllAsync();
    Task<DataResult<int>> AddAsync(T entity);
    Task<Result> UpdateAsync(T entity);
    Task<Result> UpdateStatusCodeAsync(T entity);
    Task<Result> DeleteAsync(R model);
}
