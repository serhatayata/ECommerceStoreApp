using StackExchange.Redis;

namespace IdentityServer.Api.Services.Redis.Abstract
{
    public interface IRedisService
    {
        ConnectionMultiplexer GetConnection(int db = 1);
        List<RedisKey> GetKeys(int db = 1);
        IDatabase GetDatabase(int db = 1);
        IServer GetServer();

        /// <summary>
        /// Caching with limitless time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, string value);
        /// <summary>
        /// Caching with limitless time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value) where T : class;
        /// <summary>
        /// Caching with limitless time ASYNC
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync(string key, object value);
        /// <summary>
        /// Caching with a specific period of time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        void Set(string key, object value, int duration);
        /// <summary>
        /// Caching with a specific period of time ASYNC
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        Task SetAsync(string key, object value, int duration);
        /// <summary>
        /// Caching with a specific period of time ASYNC by using databaseId
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        Task SetAsync(string key, object value, int duration, int databaseId);
        /// <summary>
        /// Gets the key if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>        
        T Get<T>(string key) where T : class;
        /// <summary>
        /// Gets the key if it exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
        /// <summary>
        /// Gets the key if it exists ASYNC
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key) where T : class;

        /// <summary>
        /// Gets the key if it exists with databaseId ASYNC
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        Task<T> GetAsyncWithDatabaseId<T>(string key, int databaseId) where T : class;

        /// <summary>
        /// Removes the key from cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        bool KeyExists(string key);
        Task<bool> KeyExistsAsync(string key);
        void RemoveByPattern(string pattern, int db = 1);
    }
}
