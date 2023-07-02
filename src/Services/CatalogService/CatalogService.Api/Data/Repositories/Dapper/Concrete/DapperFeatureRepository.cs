using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Concrete;

public class DapperFeatureRepository : IDapperFeatureRepository
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

    public Task<DataResult<IReadOnlyList<Feature>>> GetAllFeatureProperties(int featureId, int productId)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturePropertiesByProductFeatureId(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturesByProductId(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<Feature>> GetAsync(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Feature>>> GetFeatureProducts(IntModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Feature entity)
    {
        throw new NotImplementedException();
    }
}
