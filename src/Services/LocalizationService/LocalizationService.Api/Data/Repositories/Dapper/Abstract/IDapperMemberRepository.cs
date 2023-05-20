using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Data.Repositories.Dapper.Abstract
{
    public interface IDapperMemberRepository : IGenericRepository<Member,StringModel>
    {
        Task<DataResult<IReadOnlyList<Member>>> GetAllWithResourcesAsync();
        Task<DataResult<IReadOnlyList<Member>>> GetAllWithResourcesPagingAsync(PagingModel model);
        Task<DataResult<IReadOnlyList<Member>>> GetAllPagingAsync(PagingModel model);
    }
}
