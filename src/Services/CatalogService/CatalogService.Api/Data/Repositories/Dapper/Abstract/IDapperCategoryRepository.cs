using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Abstract;

public interface IDapperCategoryRepository : IDapperGenericRepository<Category, IntModel>
{
    Task<DataResult<Category>> GetWithProducts(IntModel model);
    Task<DataResult<Category>> GetByName(StringModel model);
    Task<DataResult<Category>> GetByNameWithProducts(StringModel model);
    Task<DataResult<IReadOnlyList<Category>>> GetAllByParentId(IntModel model);
    Task<DataResult<IReadOnlyList<Category>>> GetAllPagedAsync(PagingModel model);
    Task<DataResult<IReadOnlyList<Category>>> GetAllWithProductsByParentId(IntModel model);
}
