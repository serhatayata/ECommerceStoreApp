using System.Data;

namespace LocalizationService.Api.Data.Contexts.Connections.Abstract
{
    public interface ILocalizationWriteDbConnection : ILocalizationReadDbConnection
    {
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
    }
}
