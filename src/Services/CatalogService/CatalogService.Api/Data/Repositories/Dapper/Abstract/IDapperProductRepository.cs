using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Abstract;

public interface IDapperProductRepository : IGenericRepository<Product, IntModel>
{
    Task<DataResult<Product>> GetByProductCode(StringModel model);
    Task<DataResult<IReadOnlyList<Product>>> GetAllByProductTypeId(IntModel model);
    Task<DataResult<IReadOnlyList<Product>>> GetAllByBrandId(IntModel model);
    Task<DataResult<IReadOnlyList<Product>>> GetAllBetweenPrices(PriceModel model);
}
