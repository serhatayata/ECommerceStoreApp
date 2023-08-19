using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Abstract
{
    public interface IProductService
    {
        Task<Result> AddAsync(ProductAddModel entity);
        Task<Result> UpdateAsync(ProductUpdateModel entity);
        Task<Result> DeleteAsync(IntModel model);

        Task<DataResult<ProductModel>> GetAsync(IntModel model);
        Task<DataResult<IReadOnlyList<ProductModel>>> GetAllAsync();
        Task<DataResult<ProductModel>> GetByProductCodeAsync(StringModel model);
        Task<DataResult<IReadOnlyList<ProductModel>>> GetAllPagedAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<ProductModel>>> GetAllBetweenPricesAsync(PriceBetweenModel model);
        Task<DataResult<IReadOnlyList<ProductModel>>> GetAllByBrandIdAsync(IntModel model);
        Task<DataResult<IReadOnlyList<ProductModel>>> GetAllByProductTypeIdAsync(IntModel model);

        Task<DataResult<ProductSearchModel>> SearchAsync(string index, string name, bool includeSuggest = false, bool aggs = false);
        Task<DataResult<IEnumerable<ProductSuggest>>> SearchSuggestAsync(string index, string name);
        Task<Result> CreateElasticIndex(string index);
    }
}
