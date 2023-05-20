using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.LanguageModels;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.Abstract
{
    public interface ILanguageService          
    {
        Task<Result> AddAsync(LanguageAddModel model);
        Task<Result> UpdateAsync(LanguageUpdateModel model);
        Task<Result> DeleteAsync(StringModel model);
        Task<DataResult<IReadOnlyList<LanguageModel>>> GetAllWithResourcesAsync();
        Task<DataResult<IReadOnlyList<LanguageModel>>> GetAllWithResourcesPagingAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<LanguageModel>>> GetAllAsync();
        Task<DataResult<IReadOnlyList<LanguageModel>>> GetAllPagingAsync(PagingModel model);
        Task<DataResult<LanguageModel>> GetAsync(StringModel model);
    }
}
