using Newtonsoft.Json;

namespace BasketService.Api.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public static T ToObject<T>(this string value) where T : class
        {
            return string.IsNullOrEmpty(value) ? null : JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}
