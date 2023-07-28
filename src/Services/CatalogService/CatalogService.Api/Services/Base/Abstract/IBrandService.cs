using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Abstract
{
    public interface IBrandService
    {
        Task<Result> AddAsync(BrandAddModel model);
        Task<Result> UpdateAsync(BrandUpdateModel model);
        Task<Result> DeleteAsync(IntModel model);

        Task<DataResult<BrandModel>> GetAsync(IntModel model);
        Task<DataResult<IReadOnlyList<BrandModel>>> GetAllAsync();
        Task<DataResult<IReadOnlyList<BrandModel>>> GetAllPagedAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<BrandModel>>> GetAllWithProductsAsync();
    }
}
