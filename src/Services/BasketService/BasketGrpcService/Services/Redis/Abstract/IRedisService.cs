using StackExchange.Redis;

namespace BasketGrpcService.Services.Redis.Abstract
{
    public interface IRedisService
    {
        ConnectionMultiplexer GetConnection(int db = 1);
        List<RedisKey> GetKeys(int db = 1);
        RedisKey[] GetKeys(string prefix, int db = 1);
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
        Task<bool> SetAsync(string key, object value, int duration, int databaseId);
        /// <summary>
        /// Gets the key if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>        
        T Get<T>(string key) where T : class;
        /// <summary>
        /// Gets values by prefix
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        RedisValue[] GetValuesByPrefix(string prefix, int databaseId);
        /// <summary>
        /// Gets key and values by prefix
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, RedisValue> GetKeyValuesByPrefix(string prefix, int databaseId);
        /// <summary>
        /// Gets the key if it exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
        /// <summary>
        /// Gets the key if it exists by databaseId
        /// </summary>
        /// <param name="key"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        string Get(string key, int databaseId);
        /// <summary>
        /// Gets the key if it exists by databaseId
        /// </summary>
        /// <param name="key"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        T Get<T>(string key, int databaseId) where T : class;
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
        Task<T> GetAsync<T>(string key, int databaseId) where T : class;
        /// <summary>
        /// Gets the key if it exists with databaseId ASYNC using filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="duration"></param>
        /// <param name="databaseId"></param>
        /// <returns><see cref="{T}"/></returns>
        Task<T> GetAsync<T>(string key, int databaseId, int duration, Func<T> filter) where T : class;

        /// <summary>
        /// Removes the key from cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        Task<bool> RemoveAsync(string key, int databaseId);

        bool AnyKeyExistsByPrefix(string prefix, int databaseId);
        bool KeyExists(string key, int databaseId);
        bool KeyExists(string key);
        Task<bool> KeyExistsAsync(string key, int databaseId);
        Task<bool> KeyExistsAsync(string key);
        void RemoveByPattern(string pattern, int db = 1);
    }
}
