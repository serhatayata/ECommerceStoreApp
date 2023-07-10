using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfCategoryRepository : IEfCategoryRepository
{
    public Task<Result> AddAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Category>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<Category>> GetAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Category entity)
    {
        throw new NotImplementedException();
    }
}
