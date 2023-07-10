using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Concrete;

public class EfFeatureRepository : IEfFeatureRepository
{
    public Task<Result> AddAsync(Feature entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Feature>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<Feature>> GetAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Feature entity)
    {
        throw new NotImplementedException();
    }
}
