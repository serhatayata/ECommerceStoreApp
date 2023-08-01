using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Abstract;

public interface IEfFeatureRepository : IEfGenericRepository<Feature, IntModel>
{
    Task<DataResult<IReadOnlyList<Feature>>> GetAllWithProductFeaturesAsync();
    Task<Result> AddProductFeatureAsync(ProductFeature entity);
    Task<Result> AddProductFeaturePropertyAsync(ProductFeatureProperty entity);
    Task<Result> UpdateProductFeaturePropertyAsync(ProductFeatureProperty entity);
    Task<Result> DeleteProductFeatureAsync(ProductFeatureModel entity);
    Task<Result> DeleteProductFeaturePropertyAsync(IntModel entity);
}
