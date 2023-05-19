using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Data.Repositories.Dapper.Abstract
{
    public interface IDapperLanguageRepository : IGenericRepository<Language,StringModel>
    {
        Task<DataResult<IReadOnlyList<Language>>> GetAllWithResourcesAsync();
    }
}
