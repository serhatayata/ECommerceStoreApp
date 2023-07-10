using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfCommentRepository : IEfCommentRepository
{
    public Task<Result> AddAsync(Comment entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Comment>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<Comment>> GetAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Comment entity)
    {
        throw new NotImplementedException();
    }
}
