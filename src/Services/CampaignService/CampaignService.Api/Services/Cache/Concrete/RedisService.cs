using Newtonsoft.Json;
using StackExchange.Redis;
using CampaignService.Api.Services.Cache.Abstract;
using CampaignService.Api.Extensions;

namespace CampaignService.Api.Services.Cache.Concrete
{
    public class RedisService : IRedisService, IDisposable
    {
        private readonly ConnectionMultiplexer _client;
        private readonly string _connectionString;
        private readonly ConfigurationOptions _configurationOptions;

        public RedisService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetSection("RedisOptions:ConnectionString")?.Value ?? string.Empty;
            var connectionStrings = _connectionString.Split(",");

            _configurationOptions = new ConfigurationOptions()
            {
                AbortOnConnectFail = false,
                AsyncTimeout = 10000,
                ConnectTimeout = 10000,
                KeepAlive = 180
                //ServiceName = ServiceName, AllowAdmin = true
            };

            foreach (var item in connectionStrings)
            {
                _configurationOptions.EndPoints.Add(item);
            }

            _client = ConnectionMultiplexer.Connect(_configurationOptions);
        }

        public List<RedisKey> GetKeys(int db = 1)
        {
            _configurationOptions.DefaultDatabase = db;
            var _client = ConnectionMultiplexer.Connect(_configurationOptions);
            return _client.GetServer(_client.GetEndPoints().First()).Keys(db).ToList();
        }

        public RedisKey[] GetKeys(string prefix, int databaseId = 1)
        {
            var configurationOptions = GetConfigurationOptions(databaseId);
            var _client = ConnectionMultiplexer.Connect(configurationOptions);
            var keys = this.GetServer().Keys(database: databaseId, pattern: prefix + "*").ToArray();

            return keys;
        }

        public ConnectionMultiplexer GetConnection(int db = 1)
        {
            _configurationOptions.DefaultDatabase = db;
            var _client = ConnectionMultiplexer.Connect(_configurationOptions);
            return _client;
        }

        public IServer GetServer()
        {
            var server = _client.GetServer(_client.GetEndPoints().First());
            return server;
        }

        public IDatabase GetDatabase(int db = 1)
        {
            return _client.GetDatabase(db);
        }

        #region Get
        public T Get<T>(string key) where T : class
        {
            string value = _client.GetDatabase().StringGet(key);

            return value.ToObject<T>();
        }

        public string Get(string key)
        {
            return _client.GetDatabase().StringGet(key);
        }

        public string Get(string key, int databaseId)
        {
            var value = _client.GetDatabase(databaseId).StringGet(key);

            if (!string.IsNullOrWhiteSpace(value))
                value.ToString();
            return value;
        }

        public T Get<T>(string key, int databaseId) where T : class
        {
            string value = _client.GetDatabase(databaseId).StringGet(key);

            return value.ToObject<T>();
        }

        public T Get<T>(string key, int databaseId, int duration, Func<T> filter) where T : class
        {
            return _client.GetDatabase(databaseId).CacheOrGet<T>(key, duration, filter);
        }
        #endregion
        #region GetValuesByPrefix
        public RedisValue[] GetValuesByPrefix(string prefix, int databaseId = 1)
        {
            var server = this.GetServer();

            var keys = server.Keys(database: databaseId,
                                   pattern: prefix + "*").ToArray();

            RedisValue[] values = _client.GetDatabase(databaseId).StringGet(keys);
            return values;
        }
        #endregion
        #region GetKeyValuesByPrefix
        public Dictionary<string, RedisValue> GetKeyValuesByPrefix(string prefix, int databaseId = 1)
        {
            var values = new Dictionary<string, RedisValue>();

            var server = this.GetServer();

            if (!server.IsConnected)
                return values;

            var keys = server.Keys(database: databaseId,
                                   pattern: prefix + "*").ToArray();

            if (keys == null || keys?.Count() < 1)
                return values;

            foreach (var key in keys)
            {
                var value = _client.GetDatabase().StringGet(key);
                values.Add(key, value);
            }

            return values;
        }
        #endregion
        #region GetAsync<T>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            string value = await _client.GetDatabase().StringGetAsync(key);

            return value.ToObject<T>();
        }
        #endregion
        #region GetAsync<T>
        public async Task<T> GetAsync<T>(string key, int databaseId) where T : class
        {
            string value = await _client.GetDatabase(databaseId).StringGetAsync(key);

            return value.ToObject<T>();
        }
        #endregion
        #region GetAsync<T> with Func
        public async Task<T> GetAsync<T>(string key, int databaseId, int duration, Func<Task<T>> filter) where T : class
        {
            return await _client.GetDatabase(databaseId).CacheOrGetAsync<T>(key, duration, filter);
        }
        #endregion
        #region GetCompressedAsync<T> with Func
        public async Task<T> GetCompressedAsync<T>(string key, int databaseId, int duration, Func<Task<T>> filter) where T : class
        {
            return await _client.GetDatabase(databaseId).CacheOrGetCompressedAsync<T>(key, duration, filter);
        }
        #endregion
        #region Set
        public void Set(string key, string value)
        {
            _client.GetDatabase().StringSet(key, value);
        }
        #endregion
        #region Set<T>
        public void Set<T>(string key, T value) where T : class
        {
            _client.GetDatabase().StringSet(key, value.ToJson());
        }
        #endregion
        #region SetAsync
        public async Task SetAsync(string key, object value)
        {
            string jsonValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            await _client.GetDatabase().StringSetAsync(key, jsonValue);
        }
        #endregion
        #region Set
        public void Set(string key, object value, int duration)
        {
            string jsonValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            _client.GetDatabase().StringSet(key, jsonValue, TimeSpan.FromMinutes(duration));
        }
        #endregion
        #region SetAsync with TimeSpan
        public async Task SetAsync(string key, object value, int duration)
        {
            await _client.GetDatabase().StringSetAsync(key, value.ToJson(), TimeSpan.FromMinutes(duration));
        }
        #endregion
        #region SetAsync by DatabaseId and TimeSpan
        public async Task<bool> SetAsync(string key, object value, int duration, int databaseId)
        {
            var db = _client.GetDatabase(databaseId);

            string jsonValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return await db.StringSetAsync(key, jsonValue, TimeSpan.FromHours(duration));
        }
        #endregion
        #region Remove
        public void Remove(string key)
        {
            _client.GetDatabase().KeyDelete(key);
        }

        public async Task<bool> RemoveAsync(string key, int databaseId)
        {
            return await _client.GetDatabase(databaseId).KeyDeleteAsync(key);
        }
        #endregion
        #region KeyExists
        public bool KeyExists(string key)
        {
            //string value = _client.GetDatabase().StringGet(key);
            //return !string.IsNullOrEmpty(value);

            return _client.GetDatabase().KeyExists(key);
        }

        public bool KeyExists(string key, int databaseId)
        {
            return _client.GetDatabase(databaseId).KeyExists(key);
        }
        #endregion
        #region KeyExistsAsync
        public async Task<bool> KeyExistsAsync(string key)
        {
            //string value = await _client.GetDatabase().StringGetAsync(key);
            //return !string.IsNullOrEmpty(value);
            return await _client.GetDatabase().KeyExistsAsync(key);
        }

        public async Task<bool> KeyExistsAsync(string key, int databaseId)
        {
            return await _client.GetDatabase(databaseId).KeyExistsAsync(key);
        }
        #endregion
        #region AnyKeyExistsByPrefix
        public bool AnyKeyExistsByPrefix(string prefix, int databaseId)
        {
            var server = this.GetServer();

            if (!server.IsConnected)
                return false;

            var keys = server.Keys(database: databaseId,
                                   pattern: prefix + "*").ToArray();

            return keys.Count() < 1 ? false : true;
        }
        #endregion

        public void Dispose()
        {
            _client.Close();
        }

        public async Task RemoveByPattern(string pattern, int db)
        {
            _configurationOptions.DefaultDatabase = db;
            var _client = ConnectionMultiplexer.Connect(_configurationOptions);
            var server = _client.GetServer(_client.GetEndPoints().First());

            if (!server.IsConnected)
                foreach (var endpoint in _client.GetEndPoints())
                {
                    server = _client.GetServer(endpoint);

                    if (server.IsConnected)
                        break;
                }

            var keys = server.Keys();
            var values = keys.Where(x => x.ToString().Contains(pattern)).Select(c => (string)c);

            List<string> listKeys = new();
            listKeys.AddRange(values);

            foreach (var key in listKeys)
            {
                await _client.GetDatabase().KeyDeleteAsync(key);
            }
        }

        private ConfigurationOptions GetConfigurationOptions(int databaseId = 1)
        {
            var connectionStrings = _connectionString.Split(",");

            var configurationOptions = new ConfigurationOptions()
            {
                AbortOnConnectFail = false,
                AsyncTimeout = 10000,
                ConnectTimeout = 10000,
                KeepAlive = 180
                //ServiceName = ServiceName, AllowAdmin = true
            };

            foreach (var item in connectionStrings)
            {
                configurationOptions.EndPoints.Add(item);
            }

            configurationOptions.DefaultDatabase = databaseId;

            return configurationOptions;
        }
    }
}
