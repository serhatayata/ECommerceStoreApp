using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Models.Base.Concrete;

namespace LocalizationService.Api.Data.Repositories.EntityFramework.Abstract
{
    public interface IEfResourceRepository : IGenericRepository<Resource, StringModel>
    {
    }
}
