using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Abstract;

public interface IDapperProductRepository : IGenericRepository<Product, IntModel>
{
    Task<DataResult<Product>> GetByProductCodeAsync(StringModel model);
    Task<DataResult<IReadOnlyList<Product>>> GetAllByProductTypeIdAsync(IntModel model);
    Task<DataResult<IReadOnlyList<Product>>> GetAllByBrandIdAsync(IntModel model);
    Task<DataResult<IReadOnlyList<Product>>> GetAllBetweenPricesAsync(PriceBetweenModel model);
}
