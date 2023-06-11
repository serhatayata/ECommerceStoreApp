using IdentityServer.Api.Dtos.Localization;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.CacheModels;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer.Api.Utilities.Results;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Polly;
using Serilog;
using StackExchange.Redis;
using System.Reflection;

namespace IdentityServer.Api.Extensions
{
    public static class CacheExtensions
    {
        public static T CacheOrGet<T>(this StackExchange.Redis.IDatabase db,
                               string key,
                               int duration,
                               Func<T> filter) where T : class
        {
            if (!string.IsNullOrWhiteSpace(db.StringGet(key)))
            {
                var value = db.StringGet(key);
                return value.ToString().ToObject<T>();
            }
            else
            {
                var result = filter();

                if (result != null)
                {
                    string serializedValue = JsonConvert.SerializeObject(result, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    db.StringSet(key, serializedValue, TimeSpan.FromMinutes(duration));
                }

                return result;
            }
        }

        public static async Task<T> CacheOrGetAsync<T>(this StackExchange.Redis.IDatabase db,
                               string key,
                               int duration,
                               Func<T> filter) where T : class
        {
            if (!string.IsNullOrWhiteSpace(await db.StringGetAsync(key)))
            {
                var value = await db.StringGetAsync(key);
                return value.ToString().ToObject<T>();
            }
            else
            {
                var result = filter();

                if (result != null)
                {
                    string serializedValue = JsonConvert.SerializeObject(result, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    await db.StringSetAsync(key, serializedValue, TimeSpan.FromMinutes(duration));
                }

                return result;
            }
        }

        public static string GetCacheKeyByModel(CacheKeyModel model)
        {
            var parameters = string.Join("-", model.Parameters);

            var result = string.Join("-",
                             model.Prefix,
                             model.ProjectName,
                             model.ClassName,
                             model.MethodName,
                             model.Language,
                             parameters);

            return result;
        }

        public static string GetCacheKey(string[] parameters, string prefix = "")
        {
            var values = string.Join("-", parameters);

            var result = string.Join("-", prefix, values);
            return result;
        }

        public static async Task LocalizationCacheInitialize(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
            var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            var memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

            var policy = Polly.Policy.Handle<Exception>()
                        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                                ex.Message, nameof(CacheExtensions),
                                                MethodBase.GetCurrentMethod()?.Name);
                        });

            await policy.ExecuteAsync(async () =>
            {
                var values = new Dictionary<string, RedisValue>();

                var localizationMemberKey = configuration.GetSection("LocalizationSettings:MemberKey").Value;
                var localizationDuration1 = configuration.GetSection("LocalizationSettings:MemoryCache:Duration1").Get<int>();
                var localizationDuration2 = configuration.GetSection("LocalizationSettings:MemoryCache:Duration2").Get<int>();

                var localizationSuffix1 = configuration.GetSection("LocalizationSettings:MemoryCache:Suffix1").Value;
                var localizationSuffix2 = configuration.GetSection("LocalizationSettings:MemoryCache:Suffix2").Value;

                var localizationMemoryCacheDuration = configuration.GetSection("LocalizationSettings:MemoryCacheDuration").Value;
                int databaseId = configuration.GetSection("RedisSettings:LocalizationCacheDbId").Get<int>();

                if (!redisService.AnyKeyExistsByPrefix(localizationMemberKey, databaseId))
                {
                    var gatewayClient = httpClientFactory.CreateClient("gateway");
                    var result = await gatewayClient.PostGetResponseAsync<DataResult<IReadOnlyList<MemberDto>>, StringModel>("localization/members/get-all-with-resources-by-memberkey", new StringModel() { Value = localizationMemberKey });

                    if (!result.Success)
                        throw new Exception("Localization data request not successful");

                    var resultData = result.Data;

                    //MemoryCache1
                    _ = memoryCache.Set($"{localizationMemberKey}-{localizationSuffix1}", resultData, new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddHours(localizationDuration1),
                        Priority = CacheItemPriority.High
                    });

                    //MemoryCache2
                    _ = memoryCache.Set($"{localizationMemberKey}-{localizationSuffix2}", resultData, new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Now.AddHours(localizationDuration1),
                        Priority = CacheItemPriority.High
                    });

                    //REDIS SET EDILECEK
                }

                values = redisService.GetKeyValuesByPrefix(localizationMemberKey, databaseId);

                

                //Localization service istek atılacak ve redis'e kaydı yapılacak.
            });
        }
    }
}