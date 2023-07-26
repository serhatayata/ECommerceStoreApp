using CatalogService.Api.Models.CacheModels;
using Newtonsoft.Json;

namespace CatalogService.Api.Extensions
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
                               Func<Task<T>> filter) where T : class
        {
            if (!string.IsNullOrWhiteSpace(await db.StringGetAsync(key)))
            {
                var value = await db.StringGetAsync(key);
                return value.ToString().ToObject<T>();
            }
            else
            {
                var result = await filter();

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
            return string.Join("-", model.GetModelValuesAsList());
        }

        public static string GetCacheKey(string[] parameters, string prefix = "")
        {
            var values = string.Join("-", parameters);

            var result = string.Join("-", prefix, values);
            return result;
        }
    }
}
