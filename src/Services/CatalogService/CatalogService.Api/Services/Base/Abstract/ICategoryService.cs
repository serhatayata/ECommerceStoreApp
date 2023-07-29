using CatalogService.Api.Models.CategoryModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Abstract
{
    public interface ICategoryService
    {
        Task<Result> AddAsync(CategoryAddModel entity);
        Task<Result> UpdateAsync(CategoryUpdateModel entity);
    }
}
