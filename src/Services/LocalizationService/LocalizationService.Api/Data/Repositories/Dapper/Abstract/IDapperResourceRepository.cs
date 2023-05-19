using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Data.Repositories.Dapper.Abstract
{
    public interface IDapperResourceRepository : IGenericRepository<Resource, StringModel>
    {
        Task<DataResult<IReadOnlyList<Resource>>> GetAllActiveAsync();
    }
}
