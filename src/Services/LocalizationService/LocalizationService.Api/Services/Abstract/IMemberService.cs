using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.MemberModels;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.Abstract
{
    public interface IMemberService
    {
        Task<Result> AddAsync(MemberAddModel model);
        Task<Result> UpdateAsync(MemberUpdateModel model);
        Task<Result> DeleteAsync(StringModel model);
        Task<DataResult<MemberModel>> SaveToDbAsync(StringModel model);
        Task<DataResult<IReadOnlyList<MemberModel>>> GetAllWithResourcesAsync();
        Task<DataResult<IReadOnlyList<MemberModel>>> GetAllWithResourcesPagingAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<MemberModel>>> GetAllAsync();
        Task<DataResult<IReadOnlyList<MemberModel>>> GetAllPagingAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<MemberModel>>> GetAllWithResourcesByMemberKeyAsync(StringModel model);
        Task<DataResult<MemberModel>> GetAsync(StringModel model);
    }
}
