using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.ResourceModels;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.Abstract
{
    public interface IResourceService
    {
        Task<Result> AddAsync(ResourceAddModel model);
        Task<Result> UpdateAsync(ResourceUpdateModel model);
        Task<Result> DeleteAsync(StringModel model);
        Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllAsync();
        Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllActiveAsync();
        Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllPagingAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<ResourceModel>>> GetAllActivePagingAsync(PagingModel model);
        Task<DataResult<ResourceModel>> GetAsync(StringModel model);
    }
}
