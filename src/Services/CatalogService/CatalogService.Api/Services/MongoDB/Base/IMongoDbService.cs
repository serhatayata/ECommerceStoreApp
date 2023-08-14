using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.MongoDB.Base;

public interface IMongoDbService<T,R>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetAsync(R id);
    Task CreateAsync(T model);
    Task<Result> UpdateAsync(T model);
    Task<Result> DeleteAsync(R id);
}
