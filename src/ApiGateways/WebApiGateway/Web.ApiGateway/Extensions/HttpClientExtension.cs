using Web.ApiGateway.Handlers;

namespace Web.ApiGateway.Extensions
{
    public static class HttpClientExtension
    {
        public async static Task<TResult?> PostGetResponseAsync<TResult, TValue>(this HttpClient Client, string Url, TValue Value)
        {
            var httpRes = await Client.PostAsJsonAsync(Url, Value);

            return httpRes.IsSuccessStatusCode ? await httpRes.Content.ReadFromJsonAsync<TResult>() : default;
        }

        public async static Task PostAsync<TValue>(this HttpClient Client, string Url, TValue Value)
        {
            await Client.PostAsJsonAsync(Url, Value);
        }

        public async static Task<T?> GetResponseAsync<T>(this HttpClient Client, string Url)
        {
            return await Client.GetFromJsonAsync<T>(Url);
        }

        public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<HttpClientDelegatingHandler>();

            services.AddHttpClient("localization", c =>
            {
                c.BaseAddress = new Uri(configuration["ServiceInfo:Localization"]);
            }).AddHttpMessageHandler<HttpClientDelegatingHandler>();
        }
    }
}
