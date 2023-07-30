using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.EntityFramework.Abstract;

public interface IEfProductRepository : IEfGenericRepository<Product, IntModel>
{
    Task<Result> DeleteByCodeAsync(StringModel model);
    Task<DataResult<IReadOnlyList<Product>>> GetAllByBrandIdAsync(IntModel model);
    Task<DataResult<IReadOnlyList<Product>>> GetAllByProductTypeIdAsync(IntModel model);
    Task<DataResult<Product>> GetByProductCodeAsync(StringModel model);
}
