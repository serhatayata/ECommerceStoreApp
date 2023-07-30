using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Data.Repositories.Dapper.Abstract
{
    public interface IDapperGenericRepository<T, R> : IGenericRepository<T,R> where T : class
    {

    }
}
