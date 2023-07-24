using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Abstract
{
    public interface IDapperFeatureRepository : IGenericRepository<Feature, IntModel>
    {
        Task<Result> AddProductFeatureAsync(ProductFeature entity);
        Task<Result> DeleteProductFeatureAsync(IntModel entity);

        Task<Result> AddProductFeaturePropertyAsync(ProductFeatureProperty entity);
        Task<Result> DeleteProductFeaturePropertyAsync(IntModel entity);

        Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturesByProductId(IntModel model);
        Task<DataResult<IReadOnlyList<Feature>>> GetAllFeaturesByProductCode(StringModel model);
        Task<DataResult<IReadOnlyList<ProductFeatureProperty>>> GetAllFeaturePropertiesByProductFeatureId(IntModel model);
        Task<DataResult<IReadOnlyList<ProductFeatureProperty>>> GetAllFeatureProperties(int featureId, int productId);
        Task<DataResult<IReadOnlyList<Product>>> GetFeatureProducts(IntModel model);
    }
}