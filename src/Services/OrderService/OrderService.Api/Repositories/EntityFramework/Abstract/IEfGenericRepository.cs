using OrderService.Api.Repositories.Base;
using OrderService.Api.Utilities.Results;

namespace OrderService.Api.Repositories.EntityFramework.Abstract;

public interface IEfGenericRepository<T, R> : IGenericRepository<T, R> where T : class
{
    Task<DataResult<T>> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    Task<DataResult<IReadOnlyList<T>>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
}