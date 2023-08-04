using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Abstract
{
    public interface IFeatureService
    {
        Task<Result> AddAsync(FeatureAddModel entity);
        Task<Result> AddProductFeatureAsync(ProductFeatureModel entity);
        Task<Result> AddProductFeaturePropertyAsync(ProductFeaturePropertyAddModel entity);
        Task<Result> UpdateAsync(FeatureUpdateModel entity);
        Task<Result> UpdateProductFeaturePropertyAsync(ProductFeaturePropertyUpdateModel entity);
        Task<Result> DeleteAsync(IntModel model);
        Task<Result> DeleteProductFeatureAsync(ProductFeatureModel entity);
        Task<Result> DeleteProductFeaturePropertyAsync(IntModel entity);

        Task<DataResult<FeatureModel>> GetAsync(IntModel model);
        Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllAsync();
        Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllFeaturesByProductId(IntModel model);
        Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllPagedAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllFeaturesByProductCode(StringModel model);
        Task<DataResult<IReadOnlyList<ProductFeaturePropertyModel>>> GetAllFeaturePropertiesByProductFeatureId(IntModel model);
        Task<DataResult<IReadOnlyList<ProductFeaturePropertyModel>>> GetAllFeatureProperties(ProductFeatureModel model);
        Task<DataResult<IReadOnlyList<ProductModel>>> GetFeatureProducts(IntModel model);
    }
}
