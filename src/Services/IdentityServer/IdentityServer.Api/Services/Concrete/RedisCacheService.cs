using IdentityServer.Api.Extensions;
using IdentityServer.Api.Services.Abstract;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace IdentityServer.Api.Services.Concrete
{
    public class RedisCacheService : IRedisCacheService, IDisposable
    {
        private readonly ConnectionMultiplexer _client;
        private readonly string _redisDbName;
        private readonly string _connectionString;
        private readonly ConfigurationOptions _configurationOptions;


        public RedisCacheService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetSection("RedisSettings:ConnectionString")?.Value ?? string.Empty;
            _redisDbName = configuration?.GetSection("RedisSettings:DbName")?.Value ?? string.Empty;
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
            string value = _client.GetDatabase().StringGet(_redisDbName + key);

            return value.ToObject<T>();
        }
        #endregion        
        #region Get
        public string Get(string key)
        {
            return _client.GetDatabase().StringGet(_redisDbName + key);
        }
        #endregion
        #region GetAsync<T>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            string value = await _client.GetDatabase().StringGetAsync(_redisDbName + key);

            return value.ToObject<T>();
        }
        #endregion
        #region Set
        public void Set(string key, string value)
        {
            _client.GetDatabase().StringSet(_redisDbName + key, value);
        }
        #endregion
        #region Set<T>
        public void Set<T>(string key, T value) where T : class
        {
            _client.GetDatabase().StringSet(_redisDbName + key, value.ToJson());
        }
        #endregion
        #region SetAsync
        public Task SetAsync(string key, object value)
        {
            string jsonValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return _client.GetDatabase().StringSetAsync(_redisDbName + key, jsonValue);
        }
        #endregion
        #region Set
        public void Set(string key, object value, int duration)
        {
            string jsonValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            _client.GetDatabase().StringSet(_redisDbName + key, jsonValue, TimeSpan.FromMinutes(duration));
        }
        #endregion
        #region SetAsync with TimeSpan
        public Task SetAsync(string key, object value, int duration)
        {
            return _client.GetDatabase().StringSetAsync(_redisDbName + key, value.ToJson(), TimeSpan.FromMinutes(duration));
        }
        #endregion
        #region Remove
        public void Remove(string key)
        {
            _client.GetDatabase().KeyDelete(_redisDbName + key);
        }
        #endregion
        #region KeyExists
        public bool KeyExists(string key)
        {
            //string value = _client.GetDatabase().StringGet(_redisDbName + key);
            //return !string.IsNullOrEmpty(value);

            return _client.GetDatabase().KeyExists(_redisDbName + key);
        }
        #endregion
        #region KeyExistsAsync
        public async Task<bool> KeyExistsAsync(string key)
        {
            //string value = await _client.GetDatabase().StringGetAsync(_redisDbName + key);
            //return !string.IsNullOrEmpty(value);
            return await _client.GetDatabase().KeyExistsAsync(_redisDbName + key);
        }

        public void Dispose()
        {
            _client.Close();
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
