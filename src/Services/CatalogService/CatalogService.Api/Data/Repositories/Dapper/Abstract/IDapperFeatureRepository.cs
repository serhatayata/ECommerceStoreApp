using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Abstract
{
    public interface IDapperFeatureRepository : IGenericRepository<Feature, IntModel>
    {
        Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturesByProductId(IntModel model);
        Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturePropertiesByProductFeatureId(IntModel model);
        Task<DataResult<IReadOnlyList<Feature>>> GetAllFeatureProperties(int featureId, int productId);
        Task<DataResult<IReadOnlyList<Feature>>> GetFeatureProducts(IntModel model);
    }
}