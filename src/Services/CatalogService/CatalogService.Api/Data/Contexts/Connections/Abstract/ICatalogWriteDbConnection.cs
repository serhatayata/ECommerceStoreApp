using CatalogService.Api.Data.Contexts.Connections.Abstract;
using System.Data;

namespace LocalizationService.Api.Data.Contexts.Connections.Abstract
{
    public interface ICatalogWriteDbConnection : ICatalogReadDbConnection
    {
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
    }
}
