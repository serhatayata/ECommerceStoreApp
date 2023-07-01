using Nest;

namespace BasketService.Api.Services.ElasticSearch.Abstract
{
    public interface IElasticSearchService
    {
        /// <summary>
        /// Check if index exists
        /// </summary>
        /// <param name="index">index name</param>
        /// <returns><typeparamref name="T"/></returns>
        Task<bool> IndexExistsAsync(string index);
        /// <summary>
        /// Create index
        /// </summary>
        /// <param name="index">index name</param>
        /// <returns><typeparamref name="T"/></returns>
        Task<bool> CreateIndexAsync<T>(string index) where T : class;
        /// <summary>
        /// Delete index
        /// </summary>
        /// <typeparam name="T">index model</typeparam>
        /// <param name="index">index name</param>
        /// <param name="model"></param>
        /// <returns><see cref="{T}"/></returns>
        Task<bool> DeleteIndexAsync(string index);
        /// <summary>
        /// Get all 
        /// </summary>
        /// <returns><see cref="List{T}"/></returns>
        Task<List<T>> GetAllAsync<T>(string index) where T : class;
        /// <summary>
        /// Search by index and keyword
        /// </summary>
        /// <typeparam name="T">search model</typeparam>
        /// <param name="predicate">query container</param>
        /// <returns></returns>
        Task<List<T>> QueryAsync<T>(string index, QueryContainer predicate) where T : class;
        /// <summary>
        /// Search by index and keyword 
        /// </summary>
        /// <typeparam name="T">search by <see cref="{T}"/></typeparam>
        /// <param name="index">search index</param>
        /// <param name="value">search value</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <returns></returns>
        Task<List<T>> QueryPagingAsync<T>(string index, QueryContainer predicate, int page, int pageSize) where T : class;
        /// <summary>
        /// Add document
        /// </summary>
        /// <typeparam name="T">search index model</typeparam>
        /// <param name="index">index value</param>
        /// <param name="model">index model</param>
        /// <returns></returns>
        Task<bool> CreateOrUpdateAsync<T>(string index, T document) where T : class;
        /// <summary>
        /// Delete document
        /// </summary>
        /// <typeparam name="T">index model</typeparam>
        /// <param name="index">index value</param>
        /// <param name="model">model</param>
        /// <returns><see cref="{T}"/></returns>
        Task<bool> DeleteAsync<T>(string index, string key) where T : class;
    }
}
