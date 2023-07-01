using CatalogService.Api.Data.Contexts.Connections.Abstract;
using Dapper;
using LocalizationService.Api.Data.Contexts.Connections.Abstract;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LocalizationService.Api.Data.Contexts.Connections.Concrete
{
    public class CatalogReadDbConnection : ICatalogReadDbConnection, IDisposable
    {
        private readonly IDbConnection connection;
        public CatalogReadDbConnection(IConfiguration configuration)
        {
            connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await connection.QueryAsync<T>(sql, param, transaction)).AsList();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction);
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
