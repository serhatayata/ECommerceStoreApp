using LocalizationService.Api.Extensions;
using LocalizationService.Api.Services.Redis.Abstract;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LocalizationService.Api.Services.Redis.Concrete
{
    public class RedisService : IRedisService, IDisposable
    {
        private readonly ConnectionMultiplexer _client;
        private readonly string _connectionString;
        private readonly ConfigurationOptions _configurationOptions;

        public RedisService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetSection("LocalizationCacheSettings:ConnectionString")?.Value ?? string.Empty;
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

        #region Get<T>
        public T Get<T>(string key) where T : class
        {
            string value = _client.GetDatabase().StringGet(key);

            return value.ToObject<T>();
        }
        #endregion        
        #region Get
        public string Get(string key)
        {
            return _client.GetDatabase().StringGet(key);
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
        public async Task<T> GetAsync<T>(string key, int databaseId, int duration, Func<T> filter) where T : class
        {
            return await _client.GetDatabase(databaseId).CacheOrGetAsync<T>(key, duration, filter);
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
        public async Task SetAsync(string key, object value, int duration, int databaseId)
        {
            var db = _client.GetDatabase(databaseId);

            string jsonValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            await db.StringSetAsync(key, jsonValue, TimeSpan.FromHours(duration));
        }
        #endregion
        #region Remove
        public void Remove(string key)
        {
            _client.GetDatabase().KeyDelete(key);
        }
        #endregion
        #region KeyExists
        public bool KeyExists(string key)
        {
            //string value = _client.GetDatabase().StringGet(key);
            //return !string.IsNullOrEmpty(value);

            return _client.GetDatabase().KeyExists(key);
        }
        #endregion
        #region KeyExistsAsync
        public async Task<bool> KeyExistsAsync(string key)
        {
            //string value = await _client.GetDatabase().StringGetAsync(key);
            //return !string.IsNullOrEmpty(value);
            return await _client.GetDatabase().KeyExistsAsync(key);
        }

        public void Dispose()
        {
            _client.Close();
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
        #region GetAllByPrefixAsync
        public List<T> GetAllByPrefix<T>(string key, int databaseId) where T : class
        {
            try
            {
                var keys = this.GetServer().Keys(database: databaseId,
                                                 pattern: key + "*").ToList();

                var result = new List<T>();

                foreach (var keyItem in keys)
                {
                    string value = _client.GetDatabase(databaseId).StringGet(keyItem);

                    result.Add(value.ToObject<T>());
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }
        #endregion

        public async void RemoveByPattern(string pattern, int db)
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
    }
}

