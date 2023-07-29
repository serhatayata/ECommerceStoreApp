using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CategoryModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Abstract
{
    public interface ICategoryService
    {
        Task<Result> AddAsync(CategoryAddModel entity);
        Task<Result> UpdateAsync(CategoryUpdateModel entity);
        Task<Result> DeleteAsync(IntModel model);

        Task<DataResult<CategoryModel>> GetAsync(IntModel model);
        Task<DataResult<CategoryModel>> GetByName(StringModel model);
        Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllAsync();
        Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllPagedAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllByParentId(IntModel model);
        Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllWithProductsByParentId(IntModel model);
        Task<DataResult<CategoryModel>> GetWithProducts(IntModel model);
        Task<DataResult<CategoryModel>> GetByNameWithProducts(StringModel model);
    }
}
