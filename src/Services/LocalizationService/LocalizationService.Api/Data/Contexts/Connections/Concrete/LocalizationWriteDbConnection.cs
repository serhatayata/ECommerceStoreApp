using Dapper;
using LocalizationService.Api.Data.Contexts.Connections.Abstract;
using System.Data;

namespace LocalizationService.Api.Data.Contexts.Connections.Concrete
{
    public class LocalizationWriteDbConnection : ILocalizationWriteDbConnection
    {
        private readonly ILocalizationDbContext context;
        public LocalizationWriteDbConnection(ILocalizationDbContext context)
        {
            this.context = context;
        }
        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.ExecuteAsync(sql, param, transaction);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await context.Connection.QueryAsync<T>(sql, param, transaction)).AsList();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction);
        }
    }
}
